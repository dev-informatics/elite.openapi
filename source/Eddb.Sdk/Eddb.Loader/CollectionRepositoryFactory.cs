using Eddb.Sdk.Data.Core;
using Ninject.Syntax;
using Ninject;
using System.Configuration;
using Eddb.Sdk.Data.MongoDb;

namespace Eddb.Loader
{
    class CollectionRepositoryFactory
    {
        public ICollectionRepository Get(string collectionName)
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDbConnectionString"];
            var database = ConfigurationManager.AppSettings["MongoDatabase"];

            return new MongoDbCollectionRepository(connectionString, database, collectionName);
        }
    }
}
