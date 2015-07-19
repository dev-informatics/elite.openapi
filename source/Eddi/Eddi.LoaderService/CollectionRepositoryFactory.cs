using System.Configuration;
using Eddi.Data.Core;
using Eddi.Data.MongoDb;

namespace Eddi.LoaderService
{
    public class CollectionRepositoryFactory
    {
        public ICollectionRepository Get(string collectionName)
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDbConnectionString"];
            var database = ConfigurationManager.AppSettings["MongoDatabase"];

            return new MongoDbCollectionRepository(connectionString, database, collectionName);
        }

        public ICollectionRepository Get(string database, string collectionName)
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDbConnectionString"];

            return new MongoDbCollectionRepository(connectionString, database, collectionName);
        }

        public ICollectionRepository<T> Get<T>(string database, string collectionName)
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDbConnectionString"];

            return new MongoDbCollectionRepository<T>(connectionString, database, collectionName);
        }
    }
}
