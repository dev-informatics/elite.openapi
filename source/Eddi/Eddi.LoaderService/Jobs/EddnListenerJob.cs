using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eddi.Data.Core;
using Eddi.LoaderService.Providers;
using Eddi.Models.Eddi.System.V1;
using Eddi.Models.Eddn;
using Eddi.Models.Eddn.Commodities.V2;
using Eddi.Models.Eddn.Shipyard.V1;
using Eddn.Listener;
using Quartz;

namespace Eddi.LoaderService.Jobs
{
    public class EddnListenerJob : IJob
    {
        protected IEddnListener Listener { get; }

        protected CancellationToken ListenerCancellationToken { get; }

        protected CollectionRepositoryFactory RepositoryFactory { get; }

        private object SyncRoot = new object();

        private SemaphoreSlim archiveSemaphore = new SemaphoreSlim(1, 1);

        public EddnListenerJob(IEddnListener listener, CancellationTokenProvider cancellationProvider, CollectionRepositoryFactory repositoryFactory)
        {
            Listener = listener;
            ListenerCancellationToken = cancellationProvider.RetrieveOrCreate<EddnListenerJob>();
            RepositoryFactory = repositoryFactory;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public void Execute(IJobExecutionContext context)
        {
            Listener.BeginListener(async message => await ProcessMessage(message), ListenerCancellationToken, 15).Wait();

        }

        protected async Task ProcessMessage(string message)
        {
            await archiveSemaphore.WaitAsync(ListenerCancellationToken);

            if (!ListenerCancellationToken.IsCancellationRequested)
            {
                await ArchiveEddnMessageAsync(message);
                await AddNewEddiDocumentAsync(message);
            }
            archiveSemaphore.Release();

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
                await collectionRepository.SaveAsync(message.Replace("$schemaRef", "schemaRef"));
        }

        protected async Task AddNewEddiDocumentAsync(string message)
        {


            if (message.Contains("http://schemas.elite-markets.net/eddn/commodity/2"))
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommoditiesV2>(message);

                var commodities = obj.message.commodities.Select(c => new Models.Eddi.System.V1.Commodity
                {
                    commodityName = c.name,
                    buyPrice = c.buyPrice,
                    sellPrice = c.sellPrice,
                    supply = c.supply,
                    demand = c.demand,
                    supplyLevel = c.supplyLevel,
                    demandLevel = c.demandLevel
                }).ToList();

                await FindOrCreateSystemStation(obj.message.systemName, obj.message.stationName, obj.header,
                    station =>
                    {
                        station.commodities = commodities;
                        return station;
                    });

            }
            else if (message.Contains("http://schemas.elite-markets.net/eddn/shipyard/1"))
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ShipyardV1>(message);

                await FindOrCreateSystemStation(obj.message.systemName, obj.message.stationName, obj.header,
                    station =>
                    {
                        station.ships = obj.message.ships;
                        return station;
                    });
            }
        }

        protected async Task FindOrCreateSystemStation(string systemName, string stationName, Header header, Func<Station, Station> updateFunc)
        {
            var collectionRepository = RepositoryFactory.Get<SystemV1>("eddi", "systemsV1");

            var system = (await collectionRepository.FindAsync(s => s.systemName == systemName, s => s.UpdateTimestamp, -1)).FirstOrDefault()
                ?? new SystemV1
                {
                    systemName = systemName,
                    stations = new List<Station>()
                };

            system.Id = null; //Making this a new document.
            system.UpdateTimestamp = header.gatewayTimestamp;
            system.UpdaterId = header.uploaderID;
            var station = system.stations.Where(s => s.stationName == stationName).SingleOrDefault();
            if (station == null)
                system.stations.Add(updateFunc(new Station
                {
                    stationName = stationName,
                }));
            else
                updateFunc(station);

            await collectionRepository.SaveAsync(system);
        }
    }
}
