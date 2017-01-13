using System;
using System.Collections.Generic;
using SVIBEL.Core.AuthenticationClient;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Bootstrapper;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Messaging;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestBootstrapper : BootstrapperBase
	{
		private ILogger _logger;

		public TestBootstrapper()
		{
		}

		public override void SetupBootstrap()
		{
			RegisterMessagintAttributeProvider();
			RegisterCoreServices();

			var serverConfig = new TestServerConfigClient();
			serverConfig.Build(null);

			var configService = new TestConfigService();
			configService.Build(null);

			RegisterConfigService(() => configService);
			AddStaticConfigItem(serverConfig); // add the static config as it is required for the MessageService

			RegisterMessageService(MessageBrokerFactory.MakeMessageBrokerService<TestServerConfigClient>);

			//AddRequiredConfigItem()
			_logger.Log("IMessagebroker registered");
		}

		protected override void RegisterMessagintAttributeProvider()
		{
			ServiceLocator.Locator.AddServiceMapping<IMessagingAttributeProviderService>(() => new TestMessagingAttibuteProviderService());
		}

		protected override void RegisterCoreServices()
		{
			ServiceLocator.Locator.AddServiceMapping<ILogger>(() =>
			{
				SelfStartingServiceContainer<ConsoleLogger> container = new SelfStartingServiceContainer<ConsoleLogger>();
				return container.Service;
			});
			ServiceLocator.Locator.AddServiceMapping<IAuthenticationClientService>(() =>
			{
				TestAuthClientService client = new TestAuthClientService();
				client.Build(null);

				return client;
			});


			_logger = ServiceLocator.Locator.Locate<ILogger>();
			_logger.Log("Bootstrapping essenstial services...");
			_logger.Log("ILogger registered to ConsoleLogger");
		}

		public override void RegisterMessageServiceDependentServices(List<IService> services)
		{
			_preRequiredServices.AddRange(services);
			foreach (var service in _preRequiredServices)
			{
				ServiceLocator.Locator.AddServiceToLocator(service.GetType(), service);
			}

			//load additional core services here
		}
	}
}
