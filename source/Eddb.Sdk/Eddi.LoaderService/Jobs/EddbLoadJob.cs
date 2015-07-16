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

namespace Eddi.LoaderService.Jobs.Eddb
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

            //RetrieveData(downloadPath).ContinueWith(async task =>
            //    await LoadData(downloadPath)).Wait();

            RetrieveData(downloadPath).Wait();
            
            LoadData(downloadPath).Wait();
        }

        private async Task RetrieveData(string saveLocation)
        {
            var connection = ConnectionManager.CreateConnection(ConnectionManager.BaseEddbUri);

            await connection.DownloadJson(EddbConnection.ConnectionEntity.Commodities, saveLocation);
            await connection.DownloadJson(EddbConnection.ConnectionEntity.Stations_Lite, saveLocation);
            await connection.DownloadJson(EddbConnection.ConnectionEntity.Systems, saveLocation);
            await connection.DownloadJson(EddbConnection.ConnectionEntity.Stations, saveLocation);
        }

        private async Task LoadData(string saveLocation)
        {
            List<Task> deletionTasks = new List<Task>();
            List<Task> additionTasks = new List<Task>();

            var files = Directory.GetFiles(saveLocation);

            foreach (var file in files)
            {
                FileInfo fileInfo = new FileInfo(file);

                deletionTasks.Add(Repository.DropTableAsync(fileInfo.Name.Replace(".json", "")));
            }

            await Task.WhenAll(deletionTasks);

            foreach(var file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                additionTasks.Add(Repository.SaveAllAsync(file, fileInfo.Name.Replace(".json", "")));
            }

            await Task.WhenAll(additionTasks);
        }
    }
}
