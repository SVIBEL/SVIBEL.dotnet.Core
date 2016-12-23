using System;
namespace SVIBEL.Core.Models
{
	public interface IConfig : IEntity
	{
		bool IsEditable { get; set; }
	}
}
