using System;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;
using SVIBEL.Core.Models.Messaging;

namespace SVIBEL.Core.Persistance
{
	public class MessagePersistor<T> : DataProviderBackedProcessor, IStartableComponent where T: IEntity
	{
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
			SubscribeToTopic();
		}

		public override void Stop()
		{
			_messenger.UnSubscribeTopic(_subscriptionId);
		}

		private void SubscribeToTopic()
		{
			_subscriptionId = _messenger.SubscribeTopic<T>(_topic, "_Persistor_" + _topic, HandleMessage);
		}

		internal virtual void HandleMessage(IMessage<T> msg)
		{
			Provider.Insert(msg.MessageContent);
		}
	}
}
