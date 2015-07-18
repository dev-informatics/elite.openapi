using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eddb.Sdk.Data.Core;
using Eddi.LoaderService.Providers;
using Eddn.Listener;
using Quartz;

namespace Eddi.LoaderService.Jobs
{
    public class EddnListenerJob : IJob
    {
        protected IEddnListener Listener { get; }

        protected CancellationToken ListenerCancellationToken { get; }

        protected CollectionRepositoryFactory RepositoryFactory { get; }

        public EddnListenerJob(IEddnListener listener, CancellationTokenProvider cancellationProvider, CollectionRepositoryFactory repositoryFactory)
        {
            Listener = listener;
            ListenerCancellationToken = cancellationProvider.RetrieveOrCreate<EddnListenerJob>();
            RepositoryFactory = repositoryFactory;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public void Execute(IJobExecutionContext context)
        {
            Listener.BeginListener(ProcessMessage, ListenerCancellationToken, 15).Wait();
        }

        protected void ProcessMessage(string message)
        {
            Task.WaitAll(ArchiveEddnMessageAsync(message));
        }

        protected async Task ArchiveEddnMessageAsync(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            ICollectionRepository collectionRepository = null;
            if (message.Contains("http://schemas.elite-markets.net/eddn/commodity/2"))
            {
                collectionRepository = RepositoryFactory.Get("eddn", "commoditiesV2");
            }
            else if (message.Contains("http://schemas.elite-markets.net/eddn/shipyard/1"))
            {
                collectionRepository = RepositoryFactory.Get("eddn", "shipyardV1");
            }

            if (collectionRepository != null)
                await collectionRepository.SaveAsync(message);
        }

        protected async Task AddNewEddiDocumentAsync(string message)
        {
            var collectionRepository = RepositoryFactory.Get<object>("eddi", "systems");

            await collectionRepository.FindAsync(obj => obj.ToString() == "");
        }
    }
}
