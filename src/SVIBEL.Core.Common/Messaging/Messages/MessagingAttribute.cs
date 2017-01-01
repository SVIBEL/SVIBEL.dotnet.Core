using System;
using SVIBEL.Core.Common.Service;

namespace SVIBEL.Core.Common
{
	public class MessagingAttribute : Attribute
	{
		static string defaultQueueName = "SVIBEL_";
		static string defaultExchangeName = "SVIBEL_Exchange";

		static IMessagingAttributeProviderService provider = ServiceLocator.Locator.Locate<IMessagingAttributeProviderService>();

		public string QueueName { get; set; }
		public string ExchangeName { get; set; }
		public MessagingAttribute()
		{
			ExchangeName = provider != null ? provider.ExchangeName : defaultExchangeName;
			QueueName = provider != null ? provider.QueueName : defaultQueueName;
		}
	}
}
