using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Models;

namespace SVIBEL.Core.Persistance
{
	public interface IDataContext : IBuildableComponent
	{
		void Insert<T>(T data) where T : IEntity;
		IEnumerable<T> GetDataset<T>(Expression<Func<T, bool>> findExpression) where T : IEntity;
		IEnumerable<T> GetDataset<T>(FilterDefinition<T> filter) where T : IEntity;
		IEnumerable<T> GetDataset<T>() where T : IEntity;
		IEnumerable<SearchResult<T>> Search<T>(string searchText, Expression<Func<T, bool>> filter) where T : IEntity;
		T GetSingleItem<T>(Expression<Func<T, bool>> findExpression) where T : IEntity;
		void UpdateItem<T>(T newItem) where T : IEntity;
		void Delete<T>(T item) where T : IEntity;
	}
}
