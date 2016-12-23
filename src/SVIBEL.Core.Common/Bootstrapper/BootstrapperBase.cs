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
		private List<IService> _servicesToBootstrap;
		private Dictionary<Type, Func<IService>> _staticConfig;

		public BootstrapperBase()
		{
			_servicesToBootstrap = new List<IService>();
		}

		public abstract void SetupBootstrap();
		protected abstract void RegisterCoreServices();

		protected virtual void AddStaticConfig(Dictionary<Type, Func<IService>> staticConfigs)
		{
			_staticConfig = staticConfigs;
			foreach (var staticConfig in staticConfigs)
			{
				ServiceLocator.Locator.AddServiceMapping(staticConfig.Key, staticConfig.Value);
			}
		}

		protected virtual void BootstrapMessageService(Func<IService> serviceMaker)
		{
			_messageBroker = serviceMaker();
			ServiceLocator.Locator.AddServiceToLocator<IMessageBroker>(_messageBroker);

			_messageBroker.Started += MessageServiceBootstrapped;
			_messageBroker.Start(null);
		}

		protected virtual void AddServiceForBootstrap(Type serviceType, Func<IService> builder)
		{
			var service = builder();
			_servicesToBootstrap.Add(service);
			ServiceLocator.Locator.AddServiceToLocator(serviceType, service);
		}

		protected virtual void BootstrapServices()
		{
			if (_servicesToBootstrap != null && _servicesToBootstrap.Count > 0)
			{

				foreach (var service in _servicesToBootstrap)
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
			var allServicesRunning = _servicesToBootstrap.All(x => x.IsStarted);

			if (allServicesRunning)
			{
				RaiseBootstrappingFinsihed();
			}
		}
	}
}
