using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Persistance
{
	public abstract class DataProviderBackedProcessor :IStartableComponent
	{
		public event EventHandler Started;
		public IDataContext Provider { get; private set; }

		public IConfiguration<IConfig> ServerConfig
		{
			get; set;
		}

		public bool IsStarted
		{
			get;
			private set;
		}

		public DataProviderBackedProcessor(IDataContext provider)
		{
			Provider = provider;
		}


		public virtual void Start(object args)
		{
			SetConfigService();
		}

		public void RaiseStarted()
		{
			Started?.Invoke(this, EventArgs.Empty);
		}

		public virtual void Stop()
		{

		}

		// Set the config item from the config service
		// once it is set subscribe to changes so we can update this and subclasses when the config changes
		public abstract void SetConfigService();

		protected virtual void OnConfigChanged()
		{
		}
	}
}
