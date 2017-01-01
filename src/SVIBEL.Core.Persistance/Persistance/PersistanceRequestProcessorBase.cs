using System;
using System.Collections.Generic;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Service;

namespace SVIBEL.Core.Persistance
{
	public abstract class PersistanceRequestProcessorBase : DataProviderBackedProcessor, IPersistanceRequestProcessor
	{
		private IMessageBroker _messenger;
		protected List<DataProviderBackedProcessor> _messageProcessors;

		public IMessageBroker Messenger { get { return _messenger ?? (_messenger = ServiceLocator.Locator.Locate<IMessageBroker>()); } }

		protected abstract void UpdateConfigForCacheProcessors();

		public abstract void BuildPersistors();

		public PersistanceRequestProcessorBase(IDataContext provider) : base(provider)
		{
			_messageProcessors = new List<DataProviderBackedProcessor>();

			BuildPersistors();
		}

		public override void Start(object args)
		{
			_messageProcessors.ForEach(x => x.Start(null));
		}

		public override void Stop()
		{
			_messageProcessors.ForEach(x => x.Stop());
		}
	}
}
