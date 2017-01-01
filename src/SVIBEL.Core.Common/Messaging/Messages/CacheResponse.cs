using System;
using System.Collections.Generic;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Common.Messaging.Messages
{
	public class CacheResponse<T> : ICacheResponse<T> where T: IEntity
	{
		public IEnumerable<T> Payload { get; set; }
	}
}
