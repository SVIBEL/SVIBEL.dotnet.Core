using System;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Common.Messaging.Messages
{
	public class BasicMessage<T> : MessageBase<T> where T: IEntity
	{
		public BasicMessage()
		{
		}
	}
}
