using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddi.Models.Eddi.System.V1
{

    public class SystemV1
    {
        public string Id { get; set; }
        public string systemName { get; set; }
        public List<Station> stations { get; set; }

        public DateTime UpdateTimestamp { get; set; }

        public string UpdaterId { get; set; }
    }

    public class Station
    {
        public string stationName { get; set; }
        public List<Commodity> commodities { get; set; }
        public string[] ships { get; set; }
    }

    public class Commodity
    {
        public string commodityName { get; set; }
        public int supply { get; set; }
        public int buyPrice { get; set; }
        public int demand { get; set; }
        public int sellPrice { get; set; }
        public string demandLevel { get; set; }
        public string supplyLevel { get; set; }
    }

}
