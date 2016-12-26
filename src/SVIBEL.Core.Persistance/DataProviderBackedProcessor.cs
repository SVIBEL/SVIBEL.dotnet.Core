using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
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
		}

		public void RaiseStarted()
		{
			Started?.Invoke(this, EventArgs.Empty);
		}

		public virtual void Stop()
		{

		}
	}
}
