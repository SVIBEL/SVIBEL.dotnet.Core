using System;
namespace SVIBEL.Core.Models.Messaging
{
	public interface ICacheRequest<T> : IMessage<T> where T : IEntity
	{
		string ResponseTopic { get; set; }
	}
}
