using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eddn.Sdk
{
    public interface IEddnSubscriber
    {
        Task BeginListener(Action<string> callback, CancellationToken cancellationToken = default(CancellationToken));
    }
}