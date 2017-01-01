using System;
using System.Collections.Generic;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Messaging.Messages;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Persistance
{
	public abstract class CacheRequestHandlerBase<T, U, Z, W> : DataProviderBackedProcessor, IStartableComponent 
		where U : IEntity
		where Z : class, IMessage<T>
		where W : class, IMessage<U>, new()
	{
		protected ILogger _logger;
		private Guid _subscriptionId;
		private string _topic;
		private IMessageBroker _messenger;

		public IMessageBroker Messenger { get { return _messenger ?? (_messenger = ServiceLocator.Locator.Locate<IMessageBroker>()); } }

		public CacheRequestHandlerBase(IDataContext provider, string topic) : base(provider)
		{
			_topic = topic;
		}

		public override void Start(object args)
		{
			_logger = ServiceLocator.Locator.Locate<ILogger>();
			SetupCahce();
		}

		public override void Stop()
		{
		}

		internal virtual void SetupCahce()
		{
			Messenger.CacheResponder<T, U, Z, W>(_topic, "Cache_" + _topic, CacheRequestHandler);
		}
		protected abstract ICacheResponse<U> CacheRequestHandler(ICacheRequest<T> req);
		protected abstract DataEnricherBase<U> GetEnricher(IEnumerable<U> rawData);

		protected IEnumerable<U> EnrichData(IEnumerable<U> rawBasicData)
		{
			var enrichedData = rawBasicData;
			var dataEnricher = GetEnricher(rawBasicData);

			if (dataEnricher != null)
			{
				enrichedData = dataEnricher.EnrichRefData();
			}

			return enrichedData;
		}
	}
}
