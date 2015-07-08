namespace Eddb.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = Sdk.ConnectionManager.CreateConnection(Sdk.ConnectionManager.BaseEddbUri);
            connection.DownloadJson(Sdk.EddbConnection.ConnectionEntity.Systems, "C:\\Users\\michael.davidson\\downloads");
        }
    }
}
