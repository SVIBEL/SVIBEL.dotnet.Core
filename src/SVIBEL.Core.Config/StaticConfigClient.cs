using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Config
{
	public abstract class StaticConfigClient<T> : ConfigClientBase<T> where T:IConfig
	{
		public StaticConfigClient() : base()
		{
			IsConfigEditable = false;
		}

		public virtual void Build(BuildParams buildParams)
		{
			base.Build(buildParams);
			IsSnapshotReady = true;
		}
	}
}
