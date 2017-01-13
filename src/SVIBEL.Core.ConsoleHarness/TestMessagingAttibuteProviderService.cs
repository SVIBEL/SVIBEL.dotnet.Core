using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestMessagingAttibuteProviderService : IMessagingAttributeProviderService
	{
		public TestMessagingAttibuteProviderService()
		{
			QueueName = Constants.QueuePrefix;
			ExchangeName = Constants.ExchangeName;
		}

		public string ExchangeName
		{
			get;
			set;
		}

		public bool IsStarted
		{
			get;
		}

		public string QueueName
		{
			get;
			set;
		}

		public event EventHandler Started;

		public void Build(BuildParams buildParams)
		{
		}

		public void RaiseStarted()
		{
		}

		public void Start(object args)
		{
		}

		public void Stop()
		{
		}
	}
}
