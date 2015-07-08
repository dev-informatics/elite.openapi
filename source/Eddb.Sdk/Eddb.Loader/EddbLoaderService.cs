using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eddb.Loader
{
    class EddbLoaderService
    {
        private Timer _timer;
        private readonly int OneDay = 1440000;

        public EddbLoaderService()
        {
        }

        public void Start()
        {
            var nowPlusTomorrow = DateTime.Now.AddDays(1);
            var tomorrow = new DateTime(nowPlusTomorrow.Year, nowPlusTomorrow.Month, nowPlusTomorrow.Day, 0, 0, 0);
            _timer = new Timer(DoWork, null, (int)tomorrow.Subtract(DateTime.Now).TotalMilliseconds, OneDay);        
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        private void DoWork(object state)
        {
            RetrieveData();
            LoadData();   
        }

        private void RetrieveData()
        {
            var connection = Sdk.ConnectionManager.CreateConnection(Sdk.ConnectionManager.BaseEddbUri);
            var downloadPath = "f:\\eddb\\" + DateTime.Now.ToShortDateString();

            Task.Factory.StartNew(() => { connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Commodities, downloadPath); });
            Task.Factory.StartNew(() => { connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Stations, downloadPath); });
            Task.Factory.StartNew(() => { connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.StationsLite, downloadPath); });
            Task.Factory.StartNew(() => { connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Systems, downloadPath); });
        }

        private void LoadData()
        {

        }
    }
}
