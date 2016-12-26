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

	public class TestConfigService : ConfigServiceBase
	{
	}

	public class TestBootstrapper : BootstrapperBase
	{
		private ILogger _logger;

		public override void SetupBootstrap()
		{
			RegisterCoreServices();

			RegisterConfigService(() => new TestConfigService());
			var staticConfigClient = MakeRabbitMQStaticConfigClient();
			AddStaticConfigItem(staticConfigClient);
			// add the static config as it is required for the MessageService

			RegisterMessageService(MessageBrokerFactory.MakeMessageBrokerService<RabbitMQStaticConfigClient>);
			RegisterMessageServiceDependentServices();

			//AddRequiredConfigItem()

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

		private void RegisterMessageServiceDependentServices()
		{
		}
	}


	private static ILogger _logger;
	static void Main(string[] args)
	{
		TestBootstrapper bootstrapper = new TestBootstrapper();
		bootstrapper.BootstrappingFinished += BootstrapperFinished;
		bootstrapper.SetupBootstrap();
		bootstrapper.RunBootstrap();

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
			Host = "192.168.233.239",
			ExchangeName = "Thor_Exchange",
			UnauthorizedRequestTopic = MessagingConstants.UnauthorizedRequestTopic
		};
		configClient.Build(new ConfigBuildParams<MessagingConfig>() { Config = config });

		return configClient;
	}
}



