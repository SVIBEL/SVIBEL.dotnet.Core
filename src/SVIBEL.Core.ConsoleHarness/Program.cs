using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Bootstrapper;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Config;
using SVIBEL.Core.Messaging;
using SVIBEL.Core.Models;
using SVIBEL.Core.Models.Messaging;

class Program
{
	public class RabbitMQStaticConfigClient : StaticConfigClient<MessagingConfig>
	{
	}

	public class AppConfig : IConfig
	{
		public string Id
		{
			get;
			set;
		}

		public bool IsEditable
		{
			get;
			set;
		}
	}

	public class AppConfigClient : PersistedConfigClientBase<AppConfig> 
	{
		
	}

	public class TestBootstrapper : BootstrapperBase
	{
		private ILogger _logger;

		public override void SetupBootstrap()
		{
			RegisterCoreServices();

			BootstrapStaticConfig();

			RegisterMessageServiceDependentServices();

			BootstrapMessageService(MessageBrokerFactory.MakeMessageBrokerService<RabbitMQStaticConfigClient>);

			_logger.Log("IMessagebroker registered");
		}

		protected override void RegisterCoreServices()
		{
			ServiceLocator.Locator.AddServiceMapping<ILogger>(() => 
			{
				SelfStartingServiceContainer<ConsoleLogger> container = new SelfStartingServiceContainer<ConsoleLogger>();
				return container.Service;
			});
			_logger = ServiceLocator.Locator.Locate<ILogger>();
			_logger.Log("Bootstrapping essenstial services...");
			_logger.Log("ILogger registered to ConsoleLogger");
		}

		private void BootstrapStaticConfig()
		{
			var staticConfigClients = new Dictionary<Type, Func<IService>>();
			staticConfigClients.Add(typeof(RabbitMQStaticConfigClient), MakeRabbitMQStaticConfigClient);
			AddStaticConfig(staticConfigClients);
			_logger.Log("RabbitMQStaticConfigClient registered");
		}

		private void RegisterMessageServiceDependentServices()
		{
			AddServiceForBootstrap(typeof(AppConfigClient), () => 
			{
				var client = new AppConfigClient();
				var buildParams = new ConfigBuildParams<AppConfig>();
				buildParams.CacheTopic = "Thor.Cache.AppConfig";
				buildParams.LiveTopic = "Thor.Persistance.AppConfig";

				client.Build(buildParams);
				return client;
			});
		}
	}


	private static ILogger _logger;
	static void Main(string[] args)
	{
		TestBootstrapper bootstrapper = new TestBootstrapper();
		bootstrapper.BootstrappingFinished += BootstrapperFinished;
		bootstrapper.SetupBootstrap();

		Console.ReadLine();
	}

	private static void BootstrapperFinished(object sender, EventArgs e)
	{
		_logger = ServiceLocator.Locator.Locate<ILogger>();
		_logger.Log("Bootstrapping is done.");
		_logger.Log("App starting....");
	}

	private static RabbitMQStaticConfigClient MakeRabbitMQStaticConfigClient()
	{
		var configClient = new RabbitMQStaticConfigClient();
		var config = new MessagingConfig()
		{
			User = "admin",
			Password = "admin",
			Port = 5672,
			Host = "localhost",
			ExchangeName = "Thor_Exchange",
			UnauthorizedRequestTopic = MessagingConstants.UnauthorizedRequestTopic
		};
		configClient.Build(new ConfigBuildParams<MessagingConfig>() { Config = config });

		return configClient;
	}
}



