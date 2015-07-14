using Eddb.Sdk.Data.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Eddb.Sdk.Data.MongoDb
{
    public class MongoDbPersistenceRepository : IPersistenceRepository
    {
        private readonly IMongoDatabase _database;

        public MongoDbPersistenceRepository(string connectionString, string database)
        {
            _database = new MongoClient(connectionString)
                .GetDatabase(database);       
        }

        public async Task DropTableAsync(string collectionName)
        {
            await _database.DropCollectionAsync(collectionName);
        }

        public async Task SaveAllAsync(string fileName, string collectionName)
        {            
            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>(collectionName);            

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while((line = await reader.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var doc = BsonDocument.Parse(line);
                        await collection.InsertOneAsync(doc);
                    }
                }
            }
        }
    }
}
