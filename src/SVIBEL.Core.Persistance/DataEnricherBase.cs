using System;
using System.Collections.Generic;

namespace SVIBEL.Core.Persistance
{
	public abstract class DataEnricherBase<T>
	{
		protected IDataContext _provider;
		protected IEnumerable<T> _dataItems;

		public DataEnricherBase(IEnumerable<T> dataItem, IDataContext provider)
		{
			_provider = provider;
			_dataItems = dataItem;
		}

		protected abstract void SetRefDataItmes();

		public IEnumerable<T> EnrichRefData()
		{
			SetRefDataItmes();

			return _dataItems;
		}
	}
}
