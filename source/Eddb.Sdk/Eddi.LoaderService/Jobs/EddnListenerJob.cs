using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eddi.LoaderService.Providers;
using Eddn.Listener;
using Quartz;

namespace Eddi.LoaderService.Jobs
{
    public class EddnListenerJob : IJob
    {
        protected IEddnListener Listener { get; }

        protected CancellationToken ListenerCancellationToken { get; }

        public EddnListenerJob(IEddnListener listener, CancellationTokenProvider cancellationProvider)
        {
            Listener = listener;
            ListenerCancellationToken = cancellationProvider.RetrieveOrCreate<EddnListenerJob>();
        }

        public void Execute(IJobExecutionContext context)
        {
            Listener.BeginListener(ProcessMessage, ListenerCancellationToken, 15).Wait();
        }

        protected void ProcessMessage(string message)
        {
        }
    }
}
