using Eddb.Sdk.Data.Core;
using Eddb.Sdk.Data.MongoDb;
using Ninject.Modules;

namespace Eddb.Loader
{
    class LoaderModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPersistenceRepository>().To<MongoDbPersistenceRepository>().InSingletonScope().WithConstructorArgument("");
        }
    }
}
