using System;
using SVIBEL.Core.Persistance;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestCacheRequestProcessor : CacheRequestProcessorBase
	{
		public TestCacheRequestProcessor(IDataContext provider) : base(provider)
		{
		}

		public override void BuildRequestHandlers()
		{
			_cahceProcessors.Add(new SomePOCOCacheProcessor(Provider));
			//_cahceProcessors.Add(new LocationCacheProcessor(Provider));
			//_cahceProcessors.Add(new DeviceCacheProcessor(Provider));
			//_cahceProcessors.Add(new UserShiftCacheProcessor(Provider));
			//_cahceProcessors.Add(new UserEventCacheProcessor(Provider));
			//_cahceProcessors.Add(new UserHBCacheProcessor(Provider));
			//_cahceProcessors.Add(new UserSummaryForLocationCacheProcessor(Provider));
			//_cahceProcessors.Add(new UserProfileCacheProcessor(Provider));
			//_cahceProcessors.Add(new ShiftAuditCacheProcessor(Provider));
			//_cahceProcessors.Add(new ServerConfigCacheProcessor(Provider));
			//_cahceProcessors.Add(new AppConfigCacheProcessor(Provider));
			//_cahceProcessors.Add(new UserEventAndStatusCacheProcessor(Provider));
			//_cahceProcessors.Add(new ReportGenerateCacheProcessor(Provider));
			//_cahceProcessors.Add(new ReportListCacheProcessor(Provider));
			//_cahceProcessors.Add(new ReportSnapshotDetailsCacheProcessor(Provider));
			//_cahceProcessors.Add(new SearchCacheProcessor(Provider));
		}

		protected override void UpdateConfigForCacheProcessors()
		{
			foreach (var processor in _cahceProcessors)
			{
				processor.ServerConfig = ServerConfig;
			}
		}
	}
}
