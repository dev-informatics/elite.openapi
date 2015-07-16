using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eddb.Sdk.Data.Core;
using Eddb.Sdk.Data.MongoDb;
using Ninject;
using Ninject.Modules;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Eddi.LoaderService.Modules
{
    public class QuartzModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISchedulerFactory>().To<NinjectSchedulerFactory>();
            Bind<IScheduler>().ToMethod(ctx => ctx.Kernel.Get<ISchedulerFactory>().GetScheduler());
        }
    }
}
