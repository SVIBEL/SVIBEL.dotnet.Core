using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Common
{
	public interface IConfigService : IService
	{
		void AddConfig<T>(T configInstance) where T : IConfiguration;
		T GetConfig<T>() where T : IConfiguration;
		IConfiguration<T> GetConfigByModel<T>() where T : IConfig;
	}
}
