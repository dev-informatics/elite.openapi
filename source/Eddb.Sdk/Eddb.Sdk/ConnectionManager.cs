namespace Eddb.Sdk
{
    public class ConnectionManager
    {
        public const string BaseEddbUri = "http://eddb.io/archive/v3";

        public static EddbConnection CreateConnection(string connectionUri)
        {
            return new EddbConnection(connectionUri);
        }
    }
}
