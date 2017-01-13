using System;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Config;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestServerConfigClient : StaticConfigClient<ServerConfig>
	{
		public static ServerConfig GetStaticConfig()
		{
			// this is just for testing, we would probably implement something to load from a file
			var serverIp = "1ocalhost";

			var serverConfig = new ServerConfig();
			serverConfig.Database = new DBConfig { DBLocation = "test", DBIP = serverIp };
			serverConfig.Messaging = new MessagingConfig
			{
				User = "usr",
				Password = "usr",
				Port = 5672,
				Host = serverIp,
				ExchangeName = Constants.ExchangeName,
				UnauthorizedRequestTopic ="Unauthorized.User.{0}"
			};
			return serverConfig;
		}

		public TestServerConfigClient()
		{
		}

		public override void Build(BuildParams buildParams)
		{
			ConfigBuildParams<ServerConfig> serverConfigParams = new ConfigBuildParams<ServerConfig>();
			serverConfigParams.CacheTopic = Constants.Topics.GetTopic(TestTopicTypes.ServerConfigCache);
			serverConfigParams.LiveTopic = Constants.Topics.GetTopic(TestTopicTypes.ServerConfigLive);

			serverConfigParams.Config = GetStaticConfig();

			base.Build(serverConfigParams);
		}
	}
}
