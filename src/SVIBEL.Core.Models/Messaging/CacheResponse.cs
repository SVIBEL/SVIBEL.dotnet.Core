using System;
using System.Collections.Generic;

namespace SVIBEL.Core.Models.Messaging
{
	public class CacheResponse<T> : ICacheResponse<T> where T: IEntity
	{
		public IEnumerable<T> Payload { get; set; }
	}
}
