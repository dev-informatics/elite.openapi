using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Activation;

namespace Eddi.LoaderService
{
    public class NameResolutionActivatorFactory<TBinding>
    {
        protected IKernel Kernel { get; }

        protected Func<IContext, TBinding> Activator { get; }

        public NameResolutionActivatorFactory(IKernel kernel, Func<IContext, TBinding> activationFunc)
        {
            Kernel = kernel;
            Activator = activationFunc;
        }

        public TBinding Get(string name)
        {
            if (!Kernel.CanResolve<TBinding>(name))
                Kernel.Bind<TBinding>()
                    .ToMethod(Activator)
                    .InSingletonScope()
                    .Named(name);

            return Kernel.Get<TBinding>(name);
        }
    }
}
