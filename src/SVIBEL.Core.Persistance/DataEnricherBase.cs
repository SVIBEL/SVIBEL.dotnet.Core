using System;
using System.Collections.Generic;

namespace SVIBEL.Core.Persistance
{
	public abstract class DataEnricherBase<T>
	{
		internal IDataContext _provider;
		internal IEnumerable<T> _dataItems;

		public DataEnricherBase(IEnumerable<T> dataItem, IDataContext provider)
		{
			_provider = provider;
			_dataItems = dataItem;
		}

		internal abstract void SetRefDataItmes();

		public IEnumerable<T> EnrichRefData()
		{
			SetRefDataItmes();

			return _dataItems;
		}
	}
}
