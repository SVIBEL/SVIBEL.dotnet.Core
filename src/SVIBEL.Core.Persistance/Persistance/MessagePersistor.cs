using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Messaging.Messages;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Persistance
{
	public class MessagePersistor<T,U> : DataProviderBackedProcessor, IStartableComponent 
		where T: IEntity
		where U: class, IMessage<T>
	{
		protected ILogger _logger;

		private Guid _subscriptionId;
		private string _topic;
		private IMessageBroker _messenger;

		public IMessageBroker Messenger { get { return _messenger ?? (_messenger = ServiceLocator.Locator.Locate<IMessageBroker>()); } }


		public MessagePersistor(IDataContext provider, string topic) : base(provider)
		{
			_topic = topic;
		}

		public override void Start(object args)
		{
			_logger = ServiceLocator.Locator.Locate<ILogger>();
			SubscribeToTopic();
		}

		public override void Stop()
		{
			Messenger.UnSubscribeTopic(_subscriptionId);
		}

		private void SubscribeToTopic()
		{
			_subscriptionId = Messenger.SubscribeTopic<T,U>(_topic, "_Persistor_" + _topic, HandleMessage);
		}

		protected virtual void HandleMessage(IMessage<T> msg)
		{
			Provider.Insert(msg.MessageContent);
		}
	}
}
