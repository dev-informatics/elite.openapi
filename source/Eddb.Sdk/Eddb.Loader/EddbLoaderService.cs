using Eddb.Sdk.Data.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace Eddb.Loader
{
    class EddbLoaderService
    {
        private Timer _timer;
        private readonly int OneDay = ((1000 * 60) * 60) * 24;
        private readonly CollectionRepositoryFactory _factory;

        public EddbLoaderService(CollectionRepositoryFactory factory)
        {
            _factory = factory;
            DoWork(null, null);
            //_timer = new Timer(OneDay);
            //_timer.Elapsed += DoWork;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                DoWork(null, null);
                _timer.Start();
            });            
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            var downloadPath = ConfigurationManager.AppSettings["DumpDirectory"].ToString() + "\\" + DateTime.Now.ToShortDateString().Replace("/", "");

            RetrieveData(downloadPath);
            Task.WaitAll(LoadDataAsync(downloadPath));
        }

        private void RetrieveData(string saveLocation)
        {
            var connection = Sdk.ConnectionManager.CreateConnection(Sdk.ConnectionManager.BaseEddbUri);

            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Commodities, saveLocation);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Stations, saveLocation);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Stations_Lite, saveLocation);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Systems, saveLocation);
        }

        private async Task LoadDataAsync(string saveLocation)
        {
            List<Task> tasks = new List<Task>();

            foreach(var file in Directory.GetFiles(saveLocation))
            {
                FileInfo fileInfo = new FileInfo(file);
                var repository = _factory.Get(fileInfo.Name.Replace(".json", ""));
                Task.WaitAll(repository.DropCollectionAsync());                
                tasks.Add(repository.BatchSaveAsync(await LoadFromFileAsync(fileInfo.FullName)));
            }

            Task.WaitAll(tasks.ToArray()); 
        }

        private async Task<ICollection<string>> LoadFromFileAsync(string fileName)
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
