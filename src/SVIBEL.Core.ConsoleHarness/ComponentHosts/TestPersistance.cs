using System;
using System.Collections.Generic;
using SVIBEL.Core.Common.Components;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class Persistance
	{
		public class TestPersistance : ComponentHostBase
		{
			public override List<IStartableComponent> MakeComponents()
			{
				List<IStartableComponent> componets = new List<IStartableComponent>();

				TestMongoContext context = new TestMongoContext();
				context.Build(null);

				TestPersistanceService service = new TestPersistanceService(context);
				service.Build(null);
				componets.Add(service);

				return componets;

			}
		}
	}
}
