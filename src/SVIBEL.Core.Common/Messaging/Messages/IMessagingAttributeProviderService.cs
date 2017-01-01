using System;
using SVIBEL.Core.Common.Service;

namespace SVIBEL.Core.Common
{
	public interface IMessagingAttributeProviderService : IService
	{
		string QueueName { get; set; }
		string ExchangeName { get; set; }
	}
}
