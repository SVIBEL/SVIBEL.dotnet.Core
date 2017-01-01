using System;
using System.Collections.Generic;
using SVIBEL.Core.Common.Service;

namespace SVIBEL.Core.Common.Components
{
	public interface IComponentHost : IService
	{
		List<IStartableComponent> HostedComponents{get;}
	}
}
