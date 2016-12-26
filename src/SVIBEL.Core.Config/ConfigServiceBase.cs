using System;
using System.Linq;
using System.Collections.Generic;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Config
{
	public abstract class ConfigServiceBase :IConfigService, IService
	{
		public event EventHandler Started;

		private Dictionary<Type, IConfiguration> _configClients;

		public ConfigServiceBase()
		{
		}

		public bool IsStarted
		{
			get;
			private set;
		}

		public void Build(BuildParams buildParams)
		{
			if (_configClients == null)
			{
				_configClients = new Dictionary<Type, IConfiguration>();
			}
		}

		public void RaiseStarted()
		{
			Started?.Invoke(this, EventArgs.Empty);
		}

		public void Start(object args)
		{
			IsStarted = true;
			RaiseStarted();
		}

		public void Stop()
		{
			IsStarted = false;
		}

		public void AddConfig<T>(T configInstance) where T : IConfiguration
		{
			if (_configClients == null)
			{
				_configClients = new Dictionary<Type, IConfiguration>();
			}

			var configExists = _configClients.ContainsKey(typeof(T));
			if (configExists == false)
			{
				_configClients.Add(typeof(T), configInstance);
			}
		}

		public T GetConfig<T>() where T : IConfiguration
		{
			T configInstance = default(T);

			configInstance = (T)_configClients[typeof(T)]; 

			return configInstance;
		}

		public IConfiguration<T> GetConfigByModel<T>() where T : IConfig
		{
			IConfiguration<T> configInstance = default(IConfiguration<T>);

			configInstance = (IConfiguration<T>)_configClients.Where(x=> x as IConfiguration<T> != null);

			return configInstance;
		}
	}
}
