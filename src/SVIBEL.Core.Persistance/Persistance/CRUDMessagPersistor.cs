using System;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Models;
using SVIBEL.Core.Models.Messaging;

namespace SVIBEL.Core.Persistance
{
	public class CRUDMessagPersistor<T> : MessagePersistor<T> where T : IEntity
	{
		public bool IsOperationFailed { get; set; }
		public string FailReason
		{
			get; set;
		}


		public CRUDMessagPersistor(IDataContext provider, string topic) : base(provider, topic)
		{
		}

		internal override void HandleMessage(IMessage<T> msg)
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

		internal virtual void OnInsert(T data)
		{
			Provider.Insert(data);
		}

		internal virtual void OnUpdate(T data)
		{
			Provider.UpdateItem(data);
		}

		internal virtual void OnDelete(T data)
		{
			Provider.Delete<T>(data);
		}

	}
}
