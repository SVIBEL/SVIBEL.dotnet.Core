using System;

namespace SVIBEL.Core.Models
{
	public class ServerConfig : IConfig
	{
		public string Id
		{
			get; set;
		}

		public bool IsEditable
		{
			get; set;
		}

		public DBConfig Database { get; set; }

		public MessagingConfig Messaging { get; set; }

	}
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

		public string DBIP { get; set;}
	}
}
