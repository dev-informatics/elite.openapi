using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eddn.Listener;
using Ninject.Modules;

namespace Eddi.LoaderService.Modules
{
    public class EddnModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEddnListener>()
                .ToMethod(ctx => EddnListener.Create());
        }
    }
}
