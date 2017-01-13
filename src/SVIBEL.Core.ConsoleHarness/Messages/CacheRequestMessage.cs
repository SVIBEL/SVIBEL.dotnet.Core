using System;
using SVIBEL.Core.Common.Messaging.Messages;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class CacheRequestMessage<T> : TestMessageBase<T>, ICacheRequest<T>
	{
		public string ResponseTopic { get; set; }
	}
}
