using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eddi.Data.Core;
using Eddi.Data.MongoDb;
using Ninject;
using Ninject.Extensions.Quartz;
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
            Rebind<ISchedulerFactory>().To<NinjectSchedulerFactory>();
            Rebind<IScheduler>().ToMethod(ctx => ctx.Kernel.Get<ISchedulerFactory>().GetScheduler());
        }
    }
}
