using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Messaging;
using SVIBEL.Core.Models;
using SVIBEL.Core.Models.Messaging;

namespace SVIBEL.Core.Messaging
{
	public static class MessageBrokerFactory
	{
		public static IMessageBroker MakeMessageBrokerService<T>() where T : class, IConfiguration<MessagingConfig>
		{
			var broker = new RabbitMQMessageBroker<T>();
			broker.Build(null);

			return broker;
		}
	}
}
