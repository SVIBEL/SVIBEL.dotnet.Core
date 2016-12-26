using System;
namespace SVIBEL.Core.Models
{
	public class SearchResult<T> where T: IEntity
	{
		public T Entity { get; set; }
		public double TextMatchScore { get; set; }
	}
}
