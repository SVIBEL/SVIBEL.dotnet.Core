using System;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Config
{
	public class ConfigBuildParams<T> : BuildParams where T:IConfig
	{
		public T Config { get; set; }

		public ConfigBuildParams()
		{
		}
	}
}
