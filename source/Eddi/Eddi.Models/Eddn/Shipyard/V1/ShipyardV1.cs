using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddi.Models.Eddn.Shipyard.V1
{

    public class ShipyardV1
    {
        public string Id { get; set; }
        public Header header { get; set; }
        public string schemaRef { get; set; }
        public Message message { get; set; }
    }

    public class Message
    {
        public DateTime timestamp { get; set; }
        public string systemName { get; set; }
        public string stationName { get; set; }
        public string[] ships { get; set; }
    }

}
