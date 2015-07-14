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
        private readonly IPersistenceRepository _repository;

        public EddbLoaderService(IPersistenceRepository repository)
        {
            _repository = repository;
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
            LoadData(downloadPath);
        }

        private void RetrieveData(string saveLocation)
        {
            var connection = Sdk.ConnectionManager.CreateConnection(Sdk.ConnectionManager.BaseEddbUri);

            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Commodities, saveLocation);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Stations, saveLocation);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Stations_Lite, saveLocation);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Systems, saveLocation);
        }

        private void LoadData(string saveLocation)
        {
            List<Task> tasks = new List<Task>();

            foreach(var file in Directory.GetFiles(saveLocation))
            {
                FileInfo fileInfo = new FileInfo(file);
                Task.WaitAll(_repository.DropTableAsync(fileInfo.Name.Replace(".json", "")));                
                tasks.Add(_repository.SaveAllAsync(file, fileInfo.Name.Replace(".json", "")));
            }

            Task.WaitAll(tasks.ToArray()); 
        } 
    }
}
