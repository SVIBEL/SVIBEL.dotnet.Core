using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;
using SVIBEL.Core.Persistance;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestPersistanceService : PersistanceServiceBase
	{
		private IConfiguration<ServerConfig> _serverConfig;

		public TestPersistanceService(IDataContext provider) : base(provider)
		{
		}

		protected override void BuildConfig()
		{
			// persistance service only cares about server config
			// we will not do anything with app config

			var configService = ServiceLocator.Locator.Locate<IConfigService>();
			_serverConfig = configService.GetConfigByModel<ServerConfig>();

			_serverConfig.ConfigChanged += ServerConfigChanged;
		}

		protected override void BuildCacheProcessor()
		{
			_cacheProcessing = new TestCacheRequestProcessor(Provider);
		}

		protected override void BuildMessagePersistor()
		{
			_messagePeristor = new TestPersistanceRequestProcessor(Provider);
		}


		private void ServerConfigChanged(object sender, EventArgs e)
		{
			_messagePeristor.ServerConfig = _serverConfig as IConfiguration<IConfig>;
			_cacheProcessing.ServerConfig = _serverConfig as IConfiguration<IConfig>;
		}
	}
}
