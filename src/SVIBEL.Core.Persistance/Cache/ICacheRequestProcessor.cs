using System;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Persistance
{
	public interface ICacheRequestProcessor : IStartableComponent
	{
		IConfiguration<IConfig> ServerConfig { get; set; }
		void BuildRequestHandlers();
	}
}
