using System;
namespace SVIBEL.Core.Models.Messaging
{
	public abstract class MessageBase<T> : IMessage<T>, ITimestamppedEntity where T :IEntity
	{
		public DateTime Timestamp { get; set; }

		public T MessageContent { get; set; }

		public string SenderId { get; set; }

		public string Token { get; set; }
	}
}
