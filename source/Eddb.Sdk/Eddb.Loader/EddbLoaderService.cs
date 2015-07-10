using System;
using System.Configuration;
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
            var downloadPath = ConfigurationManager.AppSettings["DumpDirectory"].ToString() + "\\" + DateTime.Now.ToShortDateString().Replace("/", "");

            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Commodities, downloadPath);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Stations, downloadPath);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Stations_Lite, downloadPath);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Systems, downloadPath);
        }

        private void LoadData()
        {

        }
    }
}
