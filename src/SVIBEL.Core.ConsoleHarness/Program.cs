using System;
using System.Collections.Generic;
using System.Linq;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Service;
using SVIBEL.Core.ConsoleHarness.Mac;
class Program
{
	private static List<IComponentHost> _hostedComponents;
	private static TestBootstrapper _bootstrapper;


	static void Main(string[] args)
	{
		_hostedComponents = new List<IComponentHost>();


		_bootstrapper = new TestBootstrapper();
		_bootstrapper.BootstrappingFinished += (sender, arguments) =>
		{
			// all config an MQ are loaded, ready to go
			BuildServices();
			InitServices();
			StartServices();
		};
		_bootstrapper.SetupBootstrap();
		var persistanceServiceHost = new Persistance.TestPersistance();
		_bootstrapper.RegisterMessageServiceDependentServices(persistanceServiceHost.HostedComponents.OfType<IService>().ToList());
		_bootstrapper.RunBootstrap();

		Console.WriteLine("Waiting for messages!");
		// Keep the console window open in debug mode.
		Console.WriteLine("Press any key to exit.");
		Console.ReadKey();
	}

	private static void InitServices()
	{
		_hostedComponents.ForEach(x => x.Build(null));
	}

	private static void StartServices()
	{
		_hostedComponents.ForEach(x =>
		{
			x.Start(null);
		});
	}

	private static void BuildServices()
	{
		//this is just so every services is hosted in one exe, makes testing easy.

		//var auth = new Authentication();
		//var server = new ThorServer();
		//var diag = new DiagnosticService();
		//var modules = new ModulesService();


		_hostedComponents = new List<IComponentHost>() { };// { server, auth };//auth, diag, modules };
	}
}


