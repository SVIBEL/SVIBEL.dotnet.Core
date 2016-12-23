using System;
namespace SVIBEL.Core.Models.Messaging
{
	public class BasicMessage<T> : MessageBase<T> where T: IEntity
	{
		public BasicMessage()
		{
		}
	}
}
