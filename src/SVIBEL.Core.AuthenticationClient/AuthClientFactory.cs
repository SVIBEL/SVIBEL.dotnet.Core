using System;
namespace SVIBEL.Core.AuthenticationClient
{
	public static class AuthClientFactory
	{
		public static IAuthClient GetAuthClient()
		{
			//return new Auth0Client("https://svibel.auth0.com");
			return new DummyAuthClient();
		}
	}
}
