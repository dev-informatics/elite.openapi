using Eddb.Sdk.Data.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
