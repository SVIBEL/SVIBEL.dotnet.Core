using System;
using SVIBEL.Core.Common.Messaging.Messages;
using SVIBEL.Core.Persistance;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class SomePOCOPersistor : MessagePersistor<SomePOCO, ITestMessage<SomePOCO>>
	{

		public SomePOCOPersistor(IDataContext provider) : base(provider, Constants.Topics.GetTopic(TestTopicTypes.SomePOCOPersist))
		{
		}

		protected override void HandleMessage(IMessage<SomePOCO> msg)
		{
			base.HandleMessage(msg);
			Messenger.Publish(Constants.Topics.GetTopic(TestTopicTypes.SomePOCOLive, msg.MessageContent.Id), msg);
		}
	}
}
