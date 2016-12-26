using System;
using System.Collections.Generic;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Messaging;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;
using SVIBEL.Core.Models.Messaging;

namespace SVIBEL.Core.Persistance
{
	public abstract class CacheRequestHandlerBase<T, U> : DataProviderBackedProcessor, IStartableComponent where T:IEntity where U : IEntity
	{
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
			SetupCahce();
		}

		public override void Stop()
		{
		}

		internal virtual void SetupCahce()
		{
			_messenger.CacheResponder<T, U>(_topic, "Cache_" + _topic, CacheRequestHandler);
		}
		internal abstract ICacheResponse<U> CacheRequestHandler(ICacheRequest<T> req);
		internal abstract DataEnricherBase<U> GetEnricher(IEnumerable<U> rawData);

		internal IEnumerable<U> EnrichData(IEnumerable<U> rawBasicData)
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
