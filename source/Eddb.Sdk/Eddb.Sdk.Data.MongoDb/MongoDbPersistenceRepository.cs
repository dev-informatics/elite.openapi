using Eddb.Sdk.Data.Core;
using MongoDB.Driver;

namespace Eddb.Sdk.Data.MongoDb
{
    public class MongoDbPersistenceRepository : IPersistenceRepository
    {
        private readonly MongoClient _client;

        public MongoDbPersistenceRepository(string connectionString)
        {
            _client = new MongoClient(connectionString);
        }

        public void Save<T>(T entity)
        {
            
        }
    }
}
