using System;
using SVIBEL.Core.Common.Messaging.Messages;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestMessageBase<T> : MessageBase<T>, ITestMessage<T>, IMessage<T>
	{
		public string SomeAppWideMsgParam { get; set; }
	}
}
