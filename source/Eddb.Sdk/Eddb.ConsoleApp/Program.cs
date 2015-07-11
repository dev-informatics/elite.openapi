using System;
using System.Threading;
using Eddn.Sdk;

namespace Eddb.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var connection = Sdk.ConnectionManager.CreateConnection(Sdk.ConnectionManager.BaseEddbUri);
            //connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Systems, "C:\\Users\\michael.davidson\\downloads");

            var cts = new CancellationTokenSource();

            EddnSubscriber
                .Create()
                .BeginListener((message) => Console.WriteLine(message));


            Thread.Sleep(500000);



        }
    }
}
