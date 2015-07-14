using Eddb.Sdk.Data.Core;
using Eddb.Sdk.Data.MongoDb;
using Ninject.Modules;
using System.Configuration;

namespace Eddb.Loader
{
    class LoaderModule : NinjectModule
    {
        public override void Load()
        {
            Bind<EddbLoaderService>().ToSelf();
            Bind<IPersistenceRepository>().To<MongoDbPersistenceRepository>()
                .InSingletonScope()
                .WithConstructorArgument("connectionString", ConfigurationManager.AppSettings["MongoDbConnectionString"])
                .WithConstructorArgument("database", ConfigurationManager.AppSettings["MongoDatabase"]);
        }
    }
}
