using System;
using System.Threading.Tasks;
using System.Timers;

namespace Eddb.Loader
{
    class EddbLoaderService
    {
        private Timer _timer;
        private readonly int OneDay = ((1000 * 60) * 60) * 24;

        public EddbLoaderService()
        {
            DoWork(null, null);
            _timer = new Timer(OneDay);
            _timer.Elapsed += DoWork;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            RetrieveDataAync();
            LoadData();
        }

        private async void RetrieveDataAync()
        {
            var connection = Sdk.ConnectionManager.CreateConnection(Sdk.ConnectionManager.BaseEddbUri);
            var downloadPath = "C:\\eddb\\" + DateTime.Now.ToShortDateString().Replace("/", "");

            var task1 = Task.Factory.StartNew(() => { connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Commodities, downloadPath); });
            var task2 = Task.Factory.StartNew(() => { connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Stations, downloadPath); });

            Task.WaitAll(task1, task2);

            var task3 = Task.Factory.StartNew(() => { connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Stations_Lite, downloadPath); });
            var task4 = Task.Factory.StartNew(() => { connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Systems, downloadPath); });

            Task.WaitAll(task3, task4);
        }

        private void LoadData()
        {

        }
    }
}
