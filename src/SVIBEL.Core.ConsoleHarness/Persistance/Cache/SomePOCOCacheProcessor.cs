using System;
using System.Collections.Generic;
using System.Linq;
using SVIBEL.Core.Common.Messaging.Messages;
using SVIBEL.Core.Persistance;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class SomePOCOCacheProcessor : CacheRequestHandlerBase<SomePOCOCache, SomePOCO, ITestMessage<SomePOCOCache>, TestMessageBase<SomePOCO>>
	{

		public SomePOCOCacheProcessor(IDataContext provider) : base(provider, Constants.Topics.GetTopic(TestTopicTypes.SomePOCOCache))
		{
		}

		protected override ICacheResponse<SomePOCO> CacheRequestHandler(ICacheRequest<SomePOCOCache> req)
		{
			CacheResponse<SomePOCO> cache = new CacheResponse<SomePOCO>();


			if (req.MessageContent.CacheSearchParam)
			{
				cache.Payload = cache.Payload.Where(x => x.BlaBla == "FilteredItems");
			}

			_logger.Log("Sending user cache ---->", cache.Payload != null ? cache.Payload.Count().ToString() : "NULL");

			return cache;
		}

		protected override DataEnricherBase<SomePOCO> GetEnricher(IEnumerable<SomePOCO> rawData)
		{
			return null;
		}
	}
}
