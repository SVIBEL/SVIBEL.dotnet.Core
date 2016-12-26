using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Models;
using SVIBEL.Core.Models.Messaging;

namespace SVIBEL.Core.Persistance
{
	public abstract class PersistanceServiceBase : IStartableComponent
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

		public PersistanceServiceBase(IDataContext provider)
        {
            Provider = provider;
        }


        public virtual void Start(object args)
        {
			if (ServerConfig != null && Provider != null)
			{
				IsStarted = true;
				RaiseStarted();
			}
        }

        public void RaiseStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Stop()
        {
			IsStarted = false;
        }
	}
}
