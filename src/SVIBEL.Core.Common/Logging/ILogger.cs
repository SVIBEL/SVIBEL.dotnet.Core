using System;
using SVIBEL.Core.Common.Service;

namespace SVIBEL.Core.Common
{
	public interface ILogger : IService
	{
		void Log(string format, params string[] items);
		void Log(string log);
	}
}
