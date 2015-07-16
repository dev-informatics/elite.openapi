using Eddb.Sdk.Data.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eddb.Sdk.Data.MongoDb
{
    public class MongoDbCollectionRepository : ICollectionRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        public MongoDbCollectionRepository(string connectionString, string database, string collection)
        {
            _database = new MongoClient(connectionString)
                .GetDatabase(database);
            _collection = _database.GetCollection<BsonDocument>(collection);
            _collectionName = collection;
        }

        public async Task BatchSaveAsync(IEnumerable<string> jsonCollection)
        {
            foreach (string json in jsonCollection)
            {
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var doc = BsonDocument.Parse(json);
                    await _collection.InsertOneAsync(doc);
                }
            }
        }

        public async Task DropCollectionAsync()
        {
            await _database.DropCollectionAsync(_collectionName);
        }

        public async Task SaveAsync(string jsonString)
        {
            var doc = BsonDocument.Parse(jsonString);
            await _collection.InsertOneAsync(doc);
        }
    }
}
