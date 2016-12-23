using System;
using System.Collections.Generic;

namespace SVIBEL.Core.Common.Service
{
	public class Locator
	{
		public event EventHandler Started;

		private Dictionary<Type, Func<IService>> _serviceTypeMapping;
		private Dictionary<Type, IService> _existingServices;

		public Locator()
		{
			_serviceTypeMapping = new Dictionary<Type, Func<IService>>();
			_existingServices = new Dictionary<Type, IService>();
		}

		public void AddServiceMapping<T>(Func<IService> makeInstance) where T : IService
		{
			AddServiceMapping(typeof(T), makeInstance);
		}

		public void AddServiceMapping(Type type, Func<IService> makeInstance)
		{
			if (_serviceTypeMapping.ContainsKey(type) == false)
			{
				_serviceTypeMapping.Add(type, makeInstance);
			}
		}

		public void AddServiceToLocator<T>(IService service) where T: IService
		{
			if (_existingServices.ContainsKey(typeof(T)) == false)
			{
				_existingServices.Add(typeof(T), service);
			}
		}
		public void AddServiceToLocator(Type type, IService service)
		{
			if (_existingServices.ContainsKey(type) == false)
			{
				_existingServices.Add(type, service);
			}
		}

		public T Locate<T>() where T : class, IService
		{
			T serviceInstance = default(T);

			IService result = null;
			if (_existingServices.TryGetValue(typeof(T), out result))
			{
				serviceInstance = result as T;
			}

			if (serviceInstance == null)
			{
				if (_serviceTypeMapping.ContainsKey(typeof(T)))
				{
					var newService = _serviceTypeMapping[typeof(T)]();
					_existingServices.Add(typeof(T), newService);
					serviceInstance = (T)newService;
				}
			}

			return serviceInstance;
		}
	}

	public static class ServiceLocator
	{
		public static Locator _instance;

		public static Locator Locator
		{
			get { return _instance ?? (_instance = new Locator()); }
		}
	}
}
