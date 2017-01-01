using System;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Common.Messaging.Messages
{
	public interface ICacheRequest<T> : IMessage<T>
	{
		string ResponseTopic { get; set; }
	}
}
