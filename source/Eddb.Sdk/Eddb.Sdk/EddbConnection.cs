using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Eddb.Sdk
{
    public class EddbConnection
    {
        public enum ConnectionEntity
        {
            Commodities,
            Systems,
            Stations,
            StationsLite
        }

        private readonly string _baseUri;
        private readonly Regex _splitter = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

        internal EddbConnection(string baseUri)
        {
            _baseUri = baseUri;
        }

        public void DownloadJson(ConnectionEntity connectionEntity, string filepath)
        {
            var typePart = _splitter.Replace(connectionEntity.ToString().ToLower(), "_");            
            var uri = new Uri(_baseUri + "/" + typePart + ".json");

            WebClient client = new WebClient();
            client.DownloadFile(uri, filepath + "\\" + typePart + ".json");
        }
    }
}
