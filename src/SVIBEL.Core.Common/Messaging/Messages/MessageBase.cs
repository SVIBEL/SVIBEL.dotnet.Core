using System;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Common.Messaging.Messages
{
	public abstract class MessageBase<T> : IMessage<T>, ITimestamppedEntity
	{
		public string Id { get; set; }

		public DateTime Timestamp { get; set; }

		public T MessageContent { get; set; }

		public string SenderId { get; set; }

		public string Token { get; set; }
	}
}
