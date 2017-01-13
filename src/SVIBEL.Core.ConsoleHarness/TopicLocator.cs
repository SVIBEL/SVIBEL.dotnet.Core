
using System;
using SVIBEL.Core.Common;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TopicLocator : TopicLocator<TestTopicTypes>
	{
		public const string VirtualHostName = "";
		public const string ExchangeName = "Thor_Exchange";
		public const string QueuePrefix = "Thor";

		public const string CacheRequestPrefix = "Thor.CacheResponse.";

		private static TopicLocator _instance;
		public static TopicLocator Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TopicLocator();
					_instance.BuildTopics();
				}
				return _instance;
			}
		}

		public override void BuildTopics()
		{
			// a collection of topics supported by the system
			TopicList.Add(TestTopicTypes.SomePOCOCache, "Thor.Cache.SomePOCO.{0}");
			TopicList.Add(TestTopicTypes.SomePOCOLive, "Thor.Live.SomePOCO.{0}");
			TopicList.Add(TestTopicTypes.SomePOCOPersist, "Thor.Persist.SomePOCO.{0}");

			TopicList.Add(TestTopicTypes.ServerConfigCache, "Thor.Cache.Config.ServerConfig");
			TopicList.Add(TestTopicTypes.ServerConfigLive, "Thor.Live.Config.ServerConfig");
		}
	}
}
