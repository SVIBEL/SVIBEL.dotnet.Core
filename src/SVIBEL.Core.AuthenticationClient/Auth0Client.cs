using System;
using System.Collections.Generic;
using Auth0.AuthenticationApi;
using Auth0.Core;

namespace SVIBEL.Core.AuthenticationClient
{
	public class Auth0Client : IAuthClient
	{
		AuthenticationApiClient _authApi;

		Dictionary<string, User> _authenticatedUsers;

		public Auth0Client(string authServiceLocation)
		{
			_authApi = new AuthenticationApiClient(new Uri(authServiceLocation));
			_authenticatedUsers = new Dictionary<string, User>();
		}

		public bool IsTokenValid(string token)
		{
			var isValid = false;

			User user = null;
			if (!string.IsNullOrWhiteSpace(token))
			{
				lock (_authenticatedUsers)
				{
					if (_authenticatedUsers.ContainsKey(token))
					{
						user = _authenticatedUsers[token];
					}
					else
					{
						try
						{
							user = _authApi.GetTokenInfoAsync(token).Result;
						}
						catch
						{
							Console.WriteLine(string.Format("Invalid token:{0}", token));
						}

						if (user != null && !user.Blocked)
						{
							_authenticatedUsers.Add(token, user);
						}
					}
				}
			}


			isValid = user != null && !user.Blocked;


			return isValid;
		}
	}
}
