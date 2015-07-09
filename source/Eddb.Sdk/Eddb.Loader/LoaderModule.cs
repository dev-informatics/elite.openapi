using Eddb.Sdk.Data.Core;
using Eddb.Sdk.Data.MongoDb;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
