using System;
using SVIBEL.Core.AuthenticationClient;
using SVIBEL.Core.Common.Components;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestAuthClientService : IAuthenticationClientService
	{
		public event EventHandler Started;


		public IAuthClient AuthClient { get; set; }

		public bool IsStarted
		{
			get; private set;
		}

		public void Build(BuildParams buildParams)
		{
			AuthClient = new DummyAuthClient();
			Start(null);
		}

		public void RaiseStarted()
		{
			Started?.Invoke(this, EventArgs.Empty);
		}

		public void Start(object args)
		{
			IsStarted = true;
			RaiseStarted();
		}

		public void Stop()
		{
			IsStarted = false;
		}
	}
}
