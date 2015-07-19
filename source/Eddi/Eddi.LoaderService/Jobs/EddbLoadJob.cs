using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eddb.Sdk;
using Quartz;

namespace Eddi.LoaderService.Jobs
{
    public class EddbLoadJob : IJob
    {
        protected CollectionRepositoryFactory RepositoryFactory { get; }

        public EddbLoadJob(CollectionRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        public void Execute(IJobExecutionContext context)
        {
            var downloadPath = ConfigurationManager.AppSettings["DumpDirectory"] + "\\" + DateTime.Now.ToShortDateString().Replace("/", "");

            RetrieveData(downloadPath)
                .ContinueWith(task =>
                {
                    return LoadData(downloadPath);
                }).Unwrap().Wait();
        }

        protected async Task RetrieveData(string saveLocation)
        {
            //TODO: This should be a dependency of some sort.
            var connection = ConnectionManager.CreateConnection(ConnectionManager.BaseEddbUri);

            await connection.DownloadJson(EddbConnection.ConnectionEntity.Commodities, saveLocation);
            await connection.DownloadJson(EddbConnection.ConnectionEntity.Stations_Lite, saveLocation);
            await connection.DownloadJson(EddbConnection.ConnectionEntity.Systems, saveLocation);
            await connection.DownloadJson(EddbConnection.ConnectionEntity.Stations, saveLocation);
        }

        private async Task LoadData(string saveLocation)
        {
            await Task.WhenAll(
                new DirectoryInfo(saveLocation).GetFiles()
                    .Select(fileInfo => new
                    {
                        fileInfo,
                        repository = RepositoryFactory.Get(fileInfo.Name.Replace(".json", ""))
                    })
                    .Select(container =>
                        container.repository.DropCollectionAsync()
                            .ContinueWith(task => LoadFromFileAsync(container.fileInfo.FullName))
                            .Unwrap()
                            .ContinueWith(fileTask => container.repository.BatchSaveAsync(fileTask.Result))
                            .Unwrap()).AsParallel());
        }

        protected async Task<ICollection<string>> LoadFromFileAsync(string fileName)
        {
            List<string> jsonCollection = new List<string>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        jsonCollection.Add(line);
                    }
                }
            }

            return jsonCollection;
        }
    }
}
