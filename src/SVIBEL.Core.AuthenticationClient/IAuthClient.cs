using System;
namespace SVIBEL.Core.AuthenticationClient
{
	public interface IAuthClient
	{
		bool IsTokenValid(string token);
	}
}
