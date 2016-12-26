using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;
using SVIBEL.Core.Models.Messaging;

namespace SVIBEL.Core.Persistance
{
	public abstract class PersistanceServiceBase : IService
	{
		private PersistanceRequestProcessorBase _messagePeristor;
		private CacheRequestProcessorBase _cacheProcessing; 

		public event EventHandler Started;
        public IDataContext Provider { get; private set; }

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
			if (Provider != null)
			{
				IsStarted = true;

				_messagePeristor.Start(null);
				_cacheProcessing.Start(null);

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

		public void Build(BuildParams buildParams)
		{
			BuildConfig();
			BuildMessagePersistor();
			BuildCacheProcessor();
		}

		protected abstract void BuildConfig();
		protected abstract void BuildMessagePersistor();
		protected abstract void BuildCacheProcessor();
	}
}
