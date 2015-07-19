using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddi.Models.Eddn.Commodities.V2
{
    public class CommoditiesV2
    {
        public string Id { get; set; }
        public Header header { get; set; }
        public string schemaRef { get; set; }
        public Message message { get; set; }
    }

    public class Message
    {
        public Commodity[] commodities { get; set; }
        public DateTime timestamp { get; set; }
        public string systemName { get; set; }
        public string stationName { get; set; }
    }

    public class Commodity
    {
        public string name { get; set; }
        public int supply { get; set; }
        public int buyPrice { get; set; }
        public int demand { get; set; }
        public int sellPrice { get; set; }
        public string demandLevel { get; set; }
        public string supplyLevel { get; set; }
    }
}
