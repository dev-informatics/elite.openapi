using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eddb.Sdk.Data.Core;
using Eddb.Sdk.Data.MongoDb;
using Ninject.Modules;

namespace Eddi.LoaderService.Modules
{
    class PersistenceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPersistenceRepository>().To<MongoDbPersistenceRepository>()
                .InSingletonScope()
                .WithConstructorArgument("connectionString", ConfigurationManager.AppSettings["MongoDbConnectionString"])
                .WithConstructorArgument("database", ConfigurationManager.AppSettings["MongoDatabase"]);
            Bind<CollectionRepositoryFactory>().ToSelf();
        }

    }
}
