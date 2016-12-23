using System;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Common
{
	public interface IConfiguration<T> : IService where T : IConfig
	{
		event EventHandler ConfigChanged;
		event EventHandler ConfigReady;
		bool IsSnapshotReady { get; }
		bool IsConfigEditable { get; }

		string CacheTopic { get; }
		string LiveTopic { get; }
		T Snapshot { get; set; }
	}
}
