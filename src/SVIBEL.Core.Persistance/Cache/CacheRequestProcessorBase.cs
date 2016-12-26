using System;
using System.Collections.Generic;
using System.Linq;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Persistance
{
	public abstract class CacheRequestProcessorBase : DataProviderBackedProcessor
	{
		private IMessageBroker _messenger;
		private List<DataProviderBackedProcessor> _cahceProcessors;

		public IMessageBroker Messenger { get { return _messenger ?? (_messenger = ServiceLocator.Locator.Locate<IMessageBroker>()); } }

		protected abstract void UpdateConfigForCacheProcessors();

		protected abstract void BuildRequestHandlers();

		public CacheRequestProcessorBase(IDataContext provider) : base(provider)
		{
			_cahceProcessors = new List<DataProviderBackedProcessor>();

			BuildRequestHandlers();
		}

		public override void Start(object args)
		{
			_cahceProcessors.ForEach(x => x.Start(null));
		}

		public override void Stop()
		{
			_cahceProcessors.ForEach(x => x.Stop());
		}
	}
}
