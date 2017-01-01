using System;
using Auth0.AuthenticationApi;

namespace SVIBEL.Core.AuthenticationClient
{
	public static class AuthClientFactory
	{
		public static IAuthClient GetAuthClient(string authServiceLocation)
		{
			return new Auth0Client(authServiceLocation);
		}
	}
}
