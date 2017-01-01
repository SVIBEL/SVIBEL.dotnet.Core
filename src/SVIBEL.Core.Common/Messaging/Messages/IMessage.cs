using System;

namespace SVIBEL.Core.Common.Messaging.Messages
{
	[Messaging]
	public interface IMessage<T>
	{
		string Token { get; set; }
		T MessageContent { get; set; }
		string SenderId { get; set; }
	}
}
