using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eddi.LoaderService.Providers;
using Ninject.Modules;

namespace Eddi.LoaderService.Modules
{
    public class CancellationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<NameResolutionActivatorFactory<CancellationTokenSource>>()
                .ToMethod(ctx => new NameResolutionActivatorFactory<CancellationTokenSource>(ctx.Kernel, 
                    innerCtx => new CancellationTokenSource()));

            Bind<CancellationTokenSourceProvider>().ToSelf();

            Bind<CancellationTokenProvider>().ToSelf();
        }
    }
}
