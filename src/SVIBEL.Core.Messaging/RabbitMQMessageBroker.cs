using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using SVIBEL.Core.AuthenticationClient;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Models;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Common.Messaging.Messages;

namespace SVIBEL.Core.Messaging
{
	public class RabbitMQMessageBroker<X> : IMessageBrokerConfigured<X>
		where X : class, IConfiguration<ServerConfig>
	{
		public event EventHandler Started;

		private EasyNetQ.IEasyNetQLogger _rabbitMQLogger;
		private Dictionary<Guid, EasyNetQ.Topology.IQueue> _activeSubscriptions;
		private IAuthClient _authClient;
		private EasyNetQ.IBus _bus;
		private bool _isRunning;
		private string _unauthorizedRequestTopic;


		public bool IsAuthenticationRequired { get; protected set; }

		public bool IsConnected
		{
			get
			{
				return _isRunning && _bus.IsConnected;
			}
		}

		public X ServerConfig
		{
			get; private set;
		}

		public bool IsStarted
		{
			get { return _isRunning; }
		}

		public RabbitMQMessageBroker()
		{
			IsAuthenticationRequired = true;
#if DEBUG
			IsAuthenticationRequired = false;
#endif
		}

		public void Start(object args)
		{
			if (!_isRunning)
			{
				_isRunning = true;
				RaiseStarted();
			}
		}
		public void RaiseStarted()
		{
			if (IsConnected)
			{
				Started?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Stop()
		{
			_isRunning = false;
			_bus.Dispose();
		}

		public void Build(BuildParams buildParams)
		{
			if (!IsConnected)
			{
				_rabbitMQLogger = new RabbitLogger();

				var configService = ServiceLocator.Locator.Locate<IConfigService>();

				ServerConfig = configService.GetConfig<X>();
				_activeSubscriptions = new Dictionary<Guid, EasyNetQ.Topology.IQueue>();
				BuildAuth();

				BuildMQ(ServerConfig.Snapshot.Messaging.ConnectionString);

				_unauthorizedRequestTopic = ServerConfig.Snapshot.Messaging.UnauthorizedRequestTopic;
			}
		}

		public void UnSubscribeTopic(Guid externalId)
		{
			if (_activeSubscriptions.ContainsKey(externalId))
			{
				var sub = _activeSubscriptions[externalId];
				_activeSubscriptions.Remove(externalId);
				//sub.Dispose();
				_bus.Advanced.QueueDelete(sub);
			}
		}

		public Guid SubscribeTopic<T, U>(string topic, string subscriptionId, Action<IMessage<T>> onNext) where U : class, IMessage<T>
		{
			// All incoming messages are token validated before they are 
			// handled by the component using it.
			//var queue = _bus.Advanced.QueueDeclare("Thor_"+subscriptionId);
			//var exchange = _bus.Advanced.ExchangeDeclare(ServerConfig.Snapshot.Messaging.ExchangeName, ExchangeType.Topic);
			//var binding = _bus.Advanced.Bind(exchange, queue, topic);
			//_bus.Advanced.Consume<IMessage<T>>(queue,(response, arg2) => onNext(response.Body));

			var subscription = _bus.Subscribe<IMessage<T>>(subscriptionId, msg => HandleMessage<T,U>(onNext, msg), x => x.WithTopic(topic));
			var externaId = Guid.NewGuid();
			_activeSubscriptions.Add(externaId, subscription.Queue);

			return externaId;
		}


		public void CacheRequest<T, Z, U>(string cacheTopic, ICacheRequest<T> request, Action<ICacheResponse<Z>> onNext) 
			where Z : IEntity
			where U : class, IMessage<Z>
		{
			if (IsConnected)
			{
				var responseTopic = MessagingConstants.CacheRequestPrefix + Guid.NewGuid().ToString();
				var queue = _bus.Advanced.QueueDeclare(responseTopic, false, false, true, true, null, 10000);
				var exchange = _bus.Advanced.ExchangeDeclare(ServerConfig.Snapshot.Messaging.ExchangeName, ExchangeType.Topic);
				var binding = _bus.Advanced.Bind(exchange, queue, responseTopic);

				// setup receiving topic
				List<Z> payload = new List<Z>();
				_bus.Advanced.Consume<U>(queue, (msg, info) =>
				{
					if (Equals(msg.Body.MessageContent, default(Z)))
					{
						_bus.Advanced.QueueDelete(queue);

						// somehow we need to find out when the stream is finished
						// call the originator with full payload
						onNext(new CacheResponse<Z> { Payload = payload });
					}

					payload.Add(msg.Body.MessageContent);
				});

				request.ResponseTopic = responseTopic;

				// publish out the request
				Publish(cacheTopic, request);
			}
		}

		public void CacheResponder<T, Z, Y, W>(string topic, string name, Func<ICacheRequest<T>, ICacheResponse<Z>> getPayload)
			where Z : IEntity
			where Y : class, IMessage<T>
			where W : class, IMessage<Z>, new()
		{
			if (IsConnected)
			{
				SubscribeTopic<T, Y>(topic, name, (req) =>
				 {
					 ICacheRequest<T> cacheRequest = req as ICacheRequest<T>;

					 var payload = getPayload(cacheRequest);

					 if (payload.Payload != null)
					 {
						 foreach (var payloadItem in payload.Payload)
						 {
							 IMessage<Z> content = new W { MessageContent = payloadItem };
							 Publish<Z>(cacheRequest.ResponseTopic, content);
						 }
					 }

					 // publish empty item to signal end of stream
					 // consumer should close the private topic when this is received
					 Publish<Z>(cacheRequest.ResponseTopic, new W() { MessageContent = default(Z) });
				 });

			}
		}

		public void Publish<T>(string topic, IMessage<T> msg)
		{
			if (IsConnected)
			{
				_bus.Publish<IMessage<T>>(msg, topic);
			}
		}

		private void HandleMessage<T,U>(Action<IMessage<T>> validAction, IMessage<T> msg)
		{
			if (!IsAuthenticationRequired || TrustedToken(msg.Token) || IsMessageFromAuthenticatedUser<T>(msg))
			{
				validAction(msg);
			}
			else
			{
				PublishUnauthenticatedRequestNotification(msg);
			}
		}

		private void PublishUnauthenticatedRequestNotification<T>(IMessage<T> msg)
		{
			if (!string.IsNullOrEmpty(_unauthorizedRequestTopic) && !string.IsNullOrEmpty(msg.SenderId))
			{
				Publish(_unauthorizedRequestTopic + "." + msg.SenderId, msg);
			}
			else
			{
				Console.WriteLine("Could not publish Unauthorized Token. Topic:" + _unauthorizedRequestTopic + " SenderId:" + msg.SenderId);
			}
		}

		private bool TrustedToken(string token)
		{
			return token == MessagingConstants.ServerToken;
		}

		private bool IsMessageFromAuthenticatedUser<T>(IMessage<T> msg)
		{
			var isValid = _authClient.IsTokenValid(msg.Token);
			return isValid;
		}

		private void BuildAuth()
		{
			var authClientService = ServiceLocator.Locator.Locate<IAuthenticationClientService>();
			_authClient = authClientService.AuthClient;
		}

		private void BuildMQ(string connectionString)
		{
			_bus = EasyNetQ.RabbitHutch.CreateBus(
				connectionString,
				new EasyNetQ.AdvancedBusEventHandlers(Connected, Disconnected),
				(x) => x.Register((y) => _rabbitMQLogger)
						.Register<EasyNetQ.IConventions, AttributeBasedConventions>()
			);
		}

		private void Disconnected(object sender, EventArgs e)
		{
		}

		private void Connected(object sender, EventArgs e)
		{
			RaiseStarted();
		}

		public void SubscribeTopic<T>(string v, object onSessionRequest)
		{
			throw new NotImplementedException();
		}
	}
}
