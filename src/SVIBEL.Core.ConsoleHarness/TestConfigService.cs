using System;
using SVIBEL.Core.Config;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestAppConfigClient { }

	public class TestConfigService : ConfigServiceBase
	{
		public TestAppConfigClient AppConfig { get; set; }
		public TestServerConfigClient ServerConfig { get; set; }

		public TestConfigService()
		{
		}
	}
}
