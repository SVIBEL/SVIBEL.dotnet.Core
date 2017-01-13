using System;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestServerConfig: IEntity, IConfig
	{
		public string Id
		{
			get; set;
		}

		public bool IsEditable
		{
			get; set;
		}

		public TestServerConfig()
		{
		}
	}
}
