using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using SVIBEL.Core.Persistance;

namespace SVIBEL.Core.ConsoleHarness.Mac
{
	public class TestMongoContext : MongoContext
	{
		public TestMongoContext()
		{
		}

		protected override void SetupClassMapping()
		{
			//BsonSerializer.RegisterSerializer(new EnumSerializer<ActiveUserStates>(BsonType.String));
			//BsonSerializer.RegisterSerializer(new EnumSerializer<UserEventTypes>(BsonType.String));

			MapClass<SomePOCO>();

			BsonClassMap.RegisterClassMap<SomePOCO>();
		}

		protected override void SetupDatypeToCollectionMapping()
		{
			_dataTypeToCollectionNameMapping = new Dictionary<Type, string>();
			_dataTypeToCollectionNameMapping.Add(typeof(SomePOCO), "SomePOCO");
			//_dataTypeToCollectionNameMapping.Add(typeof(UserEvent), "UserEvent");
			//_dataTypeToCollectionNameMapping.Add(typeof(UserShift), "UserShift");
			//_dataTypeToCollectionNameMapping.Add(typeof(UserStatusEvent), "UserStatusEvent");
			//_dataTypeToCollectionNameMapping.Add(typeof(ShiftAudit), "ShiftAudit");
			//_dataTypeToCollectionNameMapping.Add(typeof(Report), "Report");
			//_dataTypeToCollectionNameMapping.Add(typeof(ReportDetails), "ReportDetails");


			//_dataTypeToCollectionNameMapping.Add(typeof(User), "Users");
			//_dataTypeToCollectionNameMapping.Add(typeof(Site), "Sites");
			//_dataTypeToCollectionNameMapping.Add(typeof(Device), "Devices");
			//_dataTypeToCollectionNameMapping.Add(typeof(AppConfig), "AppConfig");
			//_dataTypeToCollectionNameMapping.Add(typeof(ServerConfig), "ServerConfig");
		}

		private void AddTextIndexes()
		{
			try
			{
				//GetMongoDataset<UserStatusEvent>().Indexes.CreateOne(new BsonDocument("$**", "text"));
				//GetMongoDataset<UserEvent>().Indexes.CreateOne(new BsonDocument("Notes", "text"));
				//GetMongoDataset<User>().Indexes.CreateOne(new BsonDocument("$**", "text"));
				//GetMongoDataset<Site>().Indexes.CreateOne(new BsonDocument("$**", "text"));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
