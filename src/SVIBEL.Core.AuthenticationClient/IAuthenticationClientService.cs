using System;
using SVIBEL.Core.Common.Service;

namespace SVIBEL.Core.AuthenticationClient
{
	public interface IAuthenticationClientService : IService
	{
		IAuthClient AuthClient { get; set; }
	}
}
