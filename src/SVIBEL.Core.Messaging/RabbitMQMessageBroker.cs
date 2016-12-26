using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client;
using SVIBEL.Core.AuthenticationClient;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Models;
using SVIBEL.Core.Models.Messaging;
using SVIBEL.Core.Common.Service;

namespace SVIBEL.Core.Messaging
{
	public class RabbitMQMessageBroker<X> : IMessageBrokerConfigured<X> where X:class, IConfiguration<MessagingConfig>
	{
		public event EventHandler Started;

		private EasyNetQ.IEasyNetQLogger _rabbitMQLogger;
		private Dictionary<Guid, EasyNetQ.ISubscriptionResult> _activeSubscriptions;
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
				_activeSubscriptions = new Dictionary<Guid, EasyNetQ.ISubscriptionResult>();
				BuildAuth();
				BuildMQ(ServerConfig.Snapshot.ConnectionString);

				_unauthorizedRequestTopic = ServerConfig.Snapshot.UnauthorizedRequestTopic;
			}
		}

		public void UnSubscribeTopic(Guid externalId)
		{
			if (_activeSubscriptions.ContainsKey(externalId))
			{
				var sub = _activeSubscriptions[externalId];
				_activeSubscriptions.Remove(externalId);
				sub.Dispose();
				_bus.Advanced.QueueDelete(sub.Queue);
			}
		}

		public Guid SubscribeTopic<T>(string topic, string subscriptionId, Action<IMessage<T>> onNext) where T:IEntity
		{
			// All incoming messages are token validated before they are 
			// handled by the component using it.
			var subscription = _bus.Subscribe<IMessage<T>>(subscriptionId, msg => HandleMessage(onNext, msg), x => x.WithTopic(topic));
			var externaId = Guid.NewGuid();
			_activeSubscriptions.Add(externaId, subscription);

			return externaId;
		}

		public void CacheRequest<T, Z>(string cacheTopic, ICacheRequest<T> request, Action<ICacheResponse<Z>> onNext) where T: IEntity where Z : IEntity
		{
			if (IsConnected)
			{
				var responseTopic = MessagingConstants.CacheRequestPrefix + Guid.NewGuid().ToString();
				var queue = _bus.Advanced.QueueDeclare(responseTopic, false, false, true, true, null, 10000);
				var exchange = _bus.Advanced.ExchangeDeclare(ServerConfig.Snapshot.ExchangeName, ExchangeType.Topic);
				var binding = _bus.Advanced.Bind(exchange, queue, responseTopic);

				// setup receiving topic
				List<Z> payload = new List<Z>();
				_bus.Advanced.Consume<IMessage<Z>>(queue, (msg, info) =>
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

		public void CacheResponder<T, Z>(string topic, string name, Func<ICacheRequest<T>, ICacheResponse<Z>> getPayload) where T : IEntity where Z : IEntity
		{
			if (IsConnected)
			{
				SubscribeTopic<T>(topic, name, (req) =>
				{
					ICacheRequest<T> cacheRequest = req as ICacheRequest<T>;

					var payload = getPayload(cacheRequest);

					foreach (var payloadItem in payload.Payload)
					{
						Task.Run(() => {
							IMessage<Z> content = new BasicMessage<Z>() { MessageContent = payloadItem };
							Publish<Z>(cacheRequest.ResponseTopic, content);
						});
					}

					// publish empty item to signal end of stream
					// consumer should close the private topic when this is received
					Publish<Z>(cacheRequest.ResponseTopic, new BasicMessage<Z>() { MessageContent = default(Z) });
				});

			}
		}

		public void Publish<T>(string topic, IMessage<T> msg) where T : IEntity
		{
			if (IsConnected)
			{
				_bus.Publish<IMessage<T>>(msg, topic);
			}
		}

		private void HandleMessage<T>(Action<IMessage<T>> validAction, IMessage<T> msg) where T : IEntity
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

		private void PublishUnauthenticatedRequestNotification<T>(IMessage<T> msg) where T : IEntity
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

		private bool IsMessageFromAuthenticatedUser<T>(IMessage<T> msg) where T : IEntity
		{
			var isValid = _authClient.IsTokenValid(msg.Token);
			return isValid;
		}

		private void BuildAuth()
		{
			_authClient = AuthClientFactory.GetAuthClient();
		}

		private void BuildMQ(string connectionString)
		{	
			_bus = EasyNetQ.RabbitHutch.CreateBus(
				connectionString,
				new EasyNetQ.AdvancedBusEventHandlers(Connected, Disconnected),
				(x) => x.Register((y) => _rabbitMQLogger)
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
