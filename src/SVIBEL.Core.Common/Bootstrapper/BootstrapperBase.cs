using System;
using System.Linq;
using System.Collections.Generic;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Service;

namespace SVIBEL.Core.Common.Bootstrapper
{
	public abstract class BootstrapperBase
	{
		public event EventHandler BootstrappingFinished;

		private IService _messageBroker;
		private IConfigService _configService;
		private List<IService> _requiredServices;

		protected List<IService> _preRequiredServices;

		public BootstrapperBase()
		{
			_requiredServices = new List<IService>();
			_preRequiredServices = new List<IService>();
		}

		public abstract void SetupBootstrap();
		public abstract void RegisterMessageServiceDependentServices(List<IService> services);
		protected abstract void RegisterCoreServices();
		protected abstract void RegisterMessagintAttributeProvider();

		public void RunBootstrap()
		{
			_configService.Build(null);
			_configService.Start(null);

			_messageBroker.Start(null);
			// all event handlers are registered by now, once the messageBroker starts all other services will
			// start as well.
			// BootstrappingFinished will be called when all required services have started
		}

		protected virtual void AddStaticConfigItem<T>(T instance) where T : IConfiguration
		{
			instance.Build(null);
			instance.Start(null);
			_configService.AddConfig(instance);
		}

		protected virtual void AddRequiredConfigItem<T>(T instance) where T : IConfiguration
		{
			_requiredServices.Add(instance);
			_configService.AddConfig(instance);
		}


		protected virtual void RegisterConfigService(Func<IConfigService> serviceMaker)
		{
			_configService = serviceMaker();
			ServiceLocator.Locator.AddServiceToLocator<IConfigService>(_configService);
		}
		protected virtual void RegisterMessageService(Func<IMessageBroker> serviceMaker)
		{
			_messageBroker = serviceMaker();
			_messageBroker.Started += MessageServiceBootstrapped;
			ServiceLocator.Locator.AddServiceToLocator<IMessageBroker>(_messageBroker);
		}
		protected virtual void RegisterRequiredServices(Type serviceType, Func<IService> builder)
		{
			var service = builder();
			_requiredServices.Add(service);
			ServiceLocator.Locator.AddServiceToLocator(serviceType, service);
		}

		protected virtual void BootstrapServices()
		{
			if (_requiredServices != null && _requiredServices.Count > 0)
			{
				foreach (var service in _preRequiredServices)
				{
					service.Start(null);
				}

				foreach (var service in _requiredServices)
				{
					service.Started += BootstrapServiceStarted;
					service.Start(null);
				}
			}
			else 
			{
				RaiseBootstrappingFinsihed();
			}
		}

		protected virtual void RaiseBootstrappingFinsihed()
		{
			BootstrappingFinished?.Invoke(this, EventArgs.Empty);
		}

		private void MessageServiceBootstrapped(object sender, EventArgs e)
		{
		   BootstrapServices();
		}

		private void BootstrapServiceStarted(object sender, EventArgs e)
		{
			var allServicesRunning = _requiredServices.All(x => x.IsStarted);

			if (allServicesRunning)
			{
				RaiseBootstrappingFinsihed();
			}
		}
	}
}
