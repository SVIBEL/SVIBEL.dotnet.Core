using System;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Common.Service;

namespace SVIBEL.Core.Common
{
	public class SelfStartingServiceContainer<T, U> : SelfStartingServiceContainer<T> where T : IService, new() where U:BuildParams
	{
		public SelfStartingServiceContainer(U build):base()	
		{
			_buildParams = build;
			MakeService();
		}
	}

	public class SelfStartingServiceContainer<T> where T : IService, new()
	{
		public BuildParams _buildParams;
		public object _startParams;

		public T Service { get; set; }

		public SelfStartingServiceContainer()
		{
			_startParams = null;
			_buildParams = null;
			MakeService();
		}

		public virtual void MakeService()
		{
			Service = new T();
			Service.Build(_buildParams);
		}

		public virtual void StartService()
		{
			Service.Start(_startParams);
		}
	}
}
