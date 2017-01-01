using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Persistance
{
	public abstract class DataProviderBackedProcessor :IStartableComponent
	{
		private IConfiguration<IConfig> _appConfig;
		private IConfiguration<IConfig> _serverConfig;

		public event EventHandler Started;
		public IDataContext Provider { get; private set; }

		public IConfiguration<IConfig> ServerConfig
		{
			get
			{
				return _serverConfig;
			}
			set
			{
				if (_serverConfig != value)
				{
					_serverConfig = value;
					OnConfigChanged();
				}
			}
		}

		public IConfiguration<IConfig> AppConfig
		{
			get
			{
				return _appConfig;
			}
			set
			{
				if (_appConfig != value)
				{
					_appConfig = value;
					OnConfigChanged();
				}
			}
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
		}

		public void RaiseStarted()
		{
			Started?.Invoke(this, EventArgs.Empty);
		}

		public virtual void Stop()
		{

		}

		public virtual void OnConfigChanged()
		{
		}
	}
}
