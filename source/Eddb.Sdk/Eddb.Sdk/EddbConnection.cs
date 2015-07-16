using Newtonsoft.Json.Linq;
using System;
using System.IO;
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
            Stations_Lite
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
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);

            var typePart = connectionEntity.ToString().ToLower();           
            var uri = new Uri(_baseUri + "/" + typePart + ".json");

            string fileName = filepath + "\\" + typePart + ".json";
            WebClient client = new WebClient();

            using (StreamReader reader = new StreamReader(new MemoryStream(client.DownloadData(uri))))
            {
                var content = reader.ReadToEnd();
                JArray jsonArray = JArray.Parse(content);               

                using (StreamWriter writer = new StreamWriter(File.Create(fileName)))
                {
                    foreach (var item in jsonArray.Children())
                    {
                        writer.WriteLine(item.ToString(Newtonsoft.Json.Formatting.None));
                    }
                }
            }              
        }
    }
}
