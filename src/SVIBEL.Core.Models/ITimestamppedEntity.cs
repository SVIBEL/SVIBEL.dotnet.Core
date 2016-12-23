using System;
namespace SVIBEL.Core.Models
{
	public interface ITimestamppedEntity
	{
		DateTime Timestamp { get; set; }
	}
}
