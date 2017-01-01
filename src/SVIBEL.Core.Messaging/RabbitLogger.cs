using System;
using EasyNetQ;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Service;
using System.Linq;

namespace SVIBEL.Core.Messaging
{
	public class RabbitLogger : IEasyNetQLogger
	{
		private ILogger _logger;

		public RabbitLogger()
		{
			_logger = ServiceLocator.Locator.Locate<ILogger>();
		}

		public void DebugWrite(string format, params object[] args)
		{
			if (format == null || args == null)
			{
				return;
			}
			//_logger.Log(format, args.Select(x=>x.ToString()).ToArray());
		}

		public void ErrorWrite(Exception exception)
		{
			if (exception == null)
			{
				return;
			}
			_logger.Log("Exception:{0}", exception.Message);
		}

		public void ErrorWrite(string format, params object[] args)
		{
			if (format == null || args == null)
			{
				return;
			}
			_logger.Log(format);
		}

		public void InfoWrite(string format, params object[] args)
		{
			if (format == null || args == null)
			{
				return;
			}
			_logger.Log(format, args.Select(x => x.ToString()).ToArray());
		}
	}
}
