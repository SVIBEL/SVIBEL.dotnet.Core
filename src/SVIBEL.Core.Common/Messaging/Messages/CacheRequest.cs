using System;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Common.Messaging.Messages
{
	public class CacheRequest<T> : MessageBase<T>, ICacheRequest<T>
	{
		public string ResponseTopic { get; set; }
	}
}
