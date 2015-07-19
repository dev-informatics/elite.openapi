using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eddi.LoaderService.Providers
{
    public class CancellationTokenProvider
    {
        NameResolutionActivatorFactory<CancellationTokenSource> TokenSourceFactory { get; }

        public CancellationTokenProvider(NameResolutionActivatorFactory<CancellationTokenSource> factory)
        {
            TokenSourceFactory = factory;
        }

        public CancellationToken RetrieveOrCreate(string name)
        {
            return TokenSourceFactory.Get(name).Token;
        }

        public CancellationToken RetrieveOrCreate<TCancellableOperation>()
        {
            return TokenSourceFactory.Get(typeof(TCancellableOperation).AssemblyQualifiedName).Token;
        }
    }
}
