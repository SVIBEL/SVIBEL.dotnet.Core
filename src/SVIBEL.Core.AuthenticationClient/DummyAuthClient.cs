using System;
namespace SVIBEL.Core.AuthenticationClient
{
	public class DummyAuthClient : IAuthClient
	{
		public bool IsTokenValid(string token)
		{
			return true;
		}
	}
}
