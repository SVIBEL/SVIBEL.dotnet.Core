using System;
namespace SVIBEL.Core.Models.Messaging
{
	public class CacheRequest<T> : MessageBase<T>, ICacheRequest<T> where T :IEntity
	{
		public string ResponseTopic { get; set; }
	}
}
