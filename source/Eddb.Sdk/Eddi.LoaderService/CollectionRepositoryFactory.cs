using System.Configuration;
using Eddb.Sdk.Data.Core;
using Eddb.Sdk.Data.MongoDb;

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
    }
}
