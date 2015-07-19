using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eddi.LoaderService.Providers
{
    public class CancellationTokenSourceProvider
    {
        NameResolutionActivatorFactory<CancellationTokenSource> TokenSourceFactory { get; }

        public CancellationTokenSourceProvider(NameResolutionActivatorFactory<CancellationTokenSource> factory)
        {
            TokenSourceFactory = factory;
        }

        public CancellationTokenSource RetrieveOrCreate(string name)
        {
            return TokenSourceFactory.Get(name);
        }

        public CancellationTokenSource RetrieveOrCreate<TCancellableOperation>()
        {
            return TokenSourceFactory.Get(typeof(TCancellableOperation).AssemblyQualifiedName);
        }
    }
}
