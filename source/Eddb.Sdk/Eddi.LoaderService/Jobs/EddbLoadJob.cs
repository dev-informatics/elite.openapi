using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eddb.Sdk;
using Eddb.Sdk.Data.Core;
using Quartz;

namespace Eddi.LoaderService.Jobs
{
    public class EddbLoadJob : IJob
    {
        private readonly IPersistenceRepository _Repository;

        protected IPersistenceRepository Repository { get { return _Repository; } }

        public EddbLoadJob(IPersistenceRepository repository)
        {
            _Repository = repository;
        }

        public void Execute(IJobExecutionContext context)
        {
            var downloadPath = ConfigurationManager.AppSettings["DumpDirectory"].ToString() + "\\" + DateTime.Now.ToShortDateString().Replace("/", "");

            RetrieveData(downloadPath)
                .ContinueWith(task => LoadData(downloadPath))
                .Unwrap().Wait();
        }

        private async Task RetrieveData(string saveLocation)
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
                Directory.GetFiles(saveLocation)
                    .Select(file => new { file, fileInfo = new FileInfo(file) })
                    .Select(container =>
                        Repository.DropTableAsync(container.fileInfo.Name.Replace(".json", ""))
                            .ContinueWith(task =>
                                Repository.SaveAllAsync(container.file, container.fileInfo.Name.Replace(".json", "")))
                            .Unwrap()));
        }
    }
}
