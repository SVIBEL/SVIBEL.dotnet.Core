using System;
namespace SVIBEL.Core.Models
{
	public class DBConfig : IConfig
	{
		public DBConfig()
		{
		}

		public string Id
		{
			get;set;
		}

		public bool IsEditable
		{
			get;set;
		}

		public string DBLocation
		{
			get; set;
		}
	}
}
