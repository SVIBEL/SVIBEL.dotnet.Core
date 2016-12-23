using System;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Config;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Common
{
	public abstract class ConfigClientBase<T> : IConfiguration<T>, IStartableComponent, IBuildableComponent where T: IConfig
	{
		public event EventHandler ConfigChanged;
		public event EventHandler ConfigReady;
		public event EventHandler Started;

		public bool IsSnapshotReady { get; internal set;}
		public bool IsConfigEditable { get; internal set;}

		public string CacheTopic { get; internal set; }
		public string LiveTopic { get; internal set; }
		public T Snapshot { get; set; }

		public bool IsStarted
		{
			get;
			internal set;
		}

		public virtual void RaiseStarted()
		{
			Started?.Invoke(this, EventArgs.Empty);
		}
		public virtual void RaiseConfigChanged()
		{
			ConfigChanged?.Invoke(this, EventArgs.Empty);
		}
		public virtual void RaiseConfigReady()
		{
			ConfigReady?.Invoke(this, EventArgs.Empty);
		}


		public virtual void Start(object args)
		{
			IsStarted = true;
			RaiseStarted();
			if (IsSnapshotReady)
			{
				RaiseConfigReady();
			}
		}

		public virtual void Stop()
		{
			IsStarted = false;
		}

		public virtual void Build(BuildParams buildParams)
		{
			ConfigBuildParams<T> param = buildParams as ConfigBuildParams<T>;
			if (param != null)
			{
				Snapshot = param.Config;
			}
		}
	}
}
