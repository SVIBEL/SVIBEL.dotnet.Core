using System;
namespace SVIBEL.Core.Models.Messaging
{
	public abstract class UpdateRequestBase<T> : MessageBase<T> where T:IEntity
	{
		public const string OperationAdd = "add";
		public const string OperationUpdate = "update";
		public const string OperationUpdateInternal = "update_internal";
		public const string OperationDelete = "delete";

		public string Operation { get; set; }


		public bool IsRequestSuccessful { get; set; }
		public string ErrorMessage { get; set; }
	}
}
