using System;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Messaging.Messages;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Persistance
{
	public class CRUDMessagPersistor<T,U> : MessagePersistor<T, U>
		where T : IEntity
		where U : class, IMessage<T>
	{
		public bool IsOperationFailed { get; set; }
		public string FailReason
		{
			get; set;
		}


		public CRUDMessagPersistor(IDataContext provider, string topic) : base(provider, topic)
		{
		}

		protected override void HandleMessage(IMessage<T> msg)
		{
			IsOperationFailed = false;
			FailReason = string.Empty;

			var req = msg as UpdateRequestBase<T>;
			if (req != null)
			{
				switch (req.Operation)
				{
					case UpdateRequestBase<T>.OperationAdd:
						OnInsert(msg.MessageContent);
						break;
					case UpdateRequestBase<T>.OperationUpdate:
					case UpdateRequestBase<T>.OperationUpdateInternal:
						OnUpdate(msg.MessageContent);
						break;
					case UpdateRequestBase<T>.OperationDelete:
						OnDelete(msg.MessageContent);
						break;
				}
			}
		}

		protected virtual void OnInsert(T data)
		{
			Provider.Insert(data);
		}

		protected virtual void OnUpdate(T data)
		{
			Provider.UpdateItem(data);
		}

		protected virtual void OnDelete(T data)
		{
			Provider.Delete<T>(data);
		}

	}
}
