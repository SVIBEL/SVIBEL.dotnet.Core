using System;
using System.Collections.Generic;

namespace SVIBEL.Core.Common.Components
{
	public abstract class ComponentHostBase : IComponentHost
	{
		public event EventHandler Started;

		public List<IStartableComponent> HostedComponents {get;private set;}

		public bool IsStarted
		{
			get;
			private set;
		}

		public ComponentHostBase()
		{
			HostedComponents = MakeComponents();
		}

		public abstract List<IStartableComponent> MakeComponents();

		public void RaiseStarted()
		{
			Started?.Invoke(this, EventArgs.Empty);
		}

		public virtual void Start(object args)
		{
			HostedComponents.ForEach(x=>x.Start(null));
			IsStarted = true;
			RaiseStarted();
		}

		public virtual void Stop()
		{
			HostedComponents.ForEach(x=>x.Stop());
			IsStarted = false;
		}

		public virtual void Build(BuildParams buildParams)
		{
			HostedComponents = new List<IStartableComponent>();
		}
	}
}
