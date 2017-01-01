using System;
using System.Collections.Generic;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Common.Messaging.Messages
{
	public interface ICacheResponse<T> where T: IEntity
	{
		IEnumerable<T> Payload { get; set; }
	}
}
