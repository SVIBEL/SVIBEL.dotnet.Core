using System;
namespace SVIBEL.Core.Common.Components
{
	public interface IStartableComponent
	{
		event EventHandler Started;

		bool IsStarted { get; }

		void RaiseStarted();
		void Start(object args);
		void Stop();
	}
}
