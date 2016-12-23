using System;
using System.Collections.Generic;

namespace SVIBEL.Core.Models.Messaging
{
	public interface ICacheResponse<T> where T: IEntity
	{
		IEnumerable<T> Payload { get; set; }
	}
}
