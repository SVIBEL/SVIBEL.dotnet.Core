using System;
using System.Collections.Generic;
using System.Linq;

namespace SVIBEL.Core.Common
{
	public abstract class TopicLocator<T> where T : struct, IConvertible
	{
		public Dictionary<T, string> TopicList { get; set; }

		public TopicLocator()
		{
			TopicList = new Dictionary<T, string>();
		}

		public abstract void BuildTopics();

		public string GetTopic(T topicType, params string[] topicParams)
		{
			var topicNumOfParts = TopicList[topicType].Count(x => x == '{');
			if (topicParams.Count() != topicNumOfParts)
			{
				List<string> generateParamList = new List<string>();
				for (int i=0; i < topicNumOfParts; i++)
				{
					var paramExists = topicParams.Count() > i;
					if (paramExists)
					{
						generateParamList.Add(topicParams[i]);
					}
					else 
					{
						generateParamList.Add("*");
					}
				}
				topicParams = generateParamList.ToArray();
			}

			return string.Format(TopicList[topicType], topicParams);
		}
	}
}
