using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using SVIBEL.Core.Common;
using SVIBEL.Core.Common.Components;
using SVIBEL.Core.Models;
using System.Linq;
using MongoDB.Bson.Serialization;
using SVIBEL.Core.Common.Service;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;

namespace SVIBEL.Core.Persistance
{
	public abstract class MongoContext : IDataContext
	{
		protected static IMongoClient _client;
		protected static IMongoDatabase _database;

		protected Dictionary<Type, string> _dataTypeToCollectionNameMapping;

		public virtual void Build(BuildParams buildParams)
		{
			SetupDatypeToCollectionMapping();
			SetupClassMapping();

			var configService = ServiceLocator.Locator.Locate<IConfigService>();
			var serverConfig = configService.GetConfigByModel<ServerConfig>();

			var mongoUrl = MongoUrl.Create("mongodb://"+serverConfig.Snapshot.Database.DBIP+"/"+serverConfig.Snapshot.Database.DBLocation);
			_client = new MongoClient(mongoUrl);
			_database = _client.GetDatabase(mongoUrl.DatabaseName);
		}

		public void Insert<T>(T dataToInsert) where T : IEntity
		{
			var collection = GetMongoDataset<T>();
			collection.InsertOne(dataToInsert);
		}

		public IEnumerable<T> GetDataset<T>(FilterDefinition<T> filter)where T : IEntity
		{
			return GetMongoDataset<T>().Find(filter).ToEnumerable();
		}

		public IEnumerable<T> GetDataset<T>(Expression<Func<T, bool>> findExpression) where T: IEntity
		{
			return GetMongoDataset<T>().Find(findExpression).ToEnumerable();
		}

		public IEnumerable<SearchResult<T>> Search<T>(string searchTerm, Expression<Func<T, bool>> filter) where T:IEntity
		{
			//var f = Builders<T>.Filter.And(Builders<T>.Filter.Text(searchTerm), filter);
			var f = Builders<T>.Filter.Text(searchTerm ?? "");
			var p = Builders<T>.Projection.MetaTextScore("TextMatchScore");
			var s = Builders<T>.Sort.MetaTextScore("TextMatchScore");

			var sortedResult = GetMongoDataset<T>().Find(f)
													.Project(p)
													.Sort(s)
													.ToEnumerable()
													.Select(x => new SearchResult<T>
													{
														Entity = BsonSerializer.Deserialize<T>(x),
														TextMatchScore = x.ContainsValue("TextMatchScore") ? x.GetValue("TextMatchScore").AsDouble : 0
													});

			return sortedResult;
		}

		public IEnumerable<T> GetDataset<T>() where T: IEntity
		{
			return GetMongoDataset<T>().Find(x => true).ToEnumerable();
		}

		public T GetSingleItem<T>(Expression<Func<T, bool>> findExpression) where T: IEntity
		{
			return GetMongoDataset<T>().Find(findExpression).FirstOrDefault();
		}

		public void UpdateItem<T>(T item) where T : IEntity
		{
			GetMongoDataset<T>().ReplaceOne(x => x.Id == item.Id, item);
		}

		public void Delete<T>(T item) where T : IEntity
		{
			GetMongoDataset<T>().DeleteOne(x => x.Id == item.Id);
		}

		protected IMongoCollection<T> GetMongoDataset<T>() where T :IEntity
		{
			var isConnected = _client.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Connected;
			IMongoCollection<T> result = null;

			if (isConnected)
			{
				var collectionName = _dataTypeToCollectionNameMapping[typeof(T)];
				result = _database.GetCollection<T>(collectionName);
			}
			else
			{
				Console.WriteLine("Not Connected to the DB!");
			}

			return result;
		}
		protected void MapClass<T>() where T : IEntity
		{
			if (BsonClassMap.IsClassMapRegistered(typeof(T)))
			{
				return;
			}

			BsonClassMap.RegisterClassMap<T>(cm =>
			{
				cm.AutoMap();
				cm.SetIgnoreExtraElements(true);
				cm.MapIdProperty(c => c.Id)
					.SetIdGenerator(StringObjectIdGenerator.Instance)
					.SetSerializer(new StringSerializer(BsonType.ObjectId));
			});
		}

		protected abstract void SetupDatypeToCollectionMapping();
		protected abstract void SetupClassMapping();
	}
}
