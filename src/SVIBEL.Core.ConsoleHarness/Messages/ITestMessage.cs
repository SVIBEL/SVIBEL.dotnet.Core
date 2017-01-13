using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Messaging.Messages;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	[Messaging]
	public interface ITestMessage<T> : IMessage<T>
	{
		string SomeAppWideMsgParam { get; set; }
	}
}
