using System;
using SVIBEL.Core.Persistance;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestPersistanceRequestProcessor : PersistanceRequestProcessorBase
	{
		public TestPersistanceRequestProcessor(IDataContext provider) : base(provider)
		{
		}

		public override void BuildPersistors()
		{
			//events from the user
			_messageProcessors.Add(new SomePOCOPersistor(Provider));
			//_messageProcessors.Add(new UserEventMessagePersistor(Provider));
			//_messageProcessors.Add(new UserStatusEventMessagePersistor(Provider));
			//_messageProcessors.Add(new ReportSnapshotPersistor(Provider));

			////admin events
			//_messageProcessors.Add(new UserMessagePersistor(Provider));
			//_messageProcessors.Add(new SiteMessagePersistor(Provider));
			//_messageProcessors.Add(new ShiftAuditMessagePersistor(Provider));
			//_messageProcessors.Add(new UserShiftMessagePersistor(Provider));
			//_messageProcessors.Add(new ServerConfigPersistor(Provider));
			//_messageProcessors.Add(new AppConfigPersistor(Provider));
		}

		protected override void UpdateConfigForCacheProcessors()
		{
			foreach (var processor in _messageProcessors)
			{
				processor.ServerConfig = ServerConfig;
			}
		}
	}
}
