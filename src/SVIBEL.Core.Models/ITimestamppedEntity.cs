using System;
namespace SVIBEL.Core.Models
{
	public interface ITimestamppedEntity : IEntity
	{
		DateTime Timestamp { get; set; }
	}
}
