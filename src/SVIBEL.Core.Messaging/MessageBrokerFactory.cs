using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Messaging
{
	public static class MessageBrokerFactory
	{
		public static IMessageBroker MakeMessageBrokerService<T>() where T : class, IConfiguration<ServerConfig>
		{
			var broker = new RabbitMQMessageBroker<T>();
			broker.Build(null);

			return broker;
		}
	}
}
