using System;
namespace SVIBEL.Core.Models.Messaging
{
	public interface IMessage<T> where T: IEntity
	{
		string Token { get; set; }
		T MessageContent { get; set; }
		string SenderId { get; set; }
	}
}
