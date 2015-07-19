using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddi.Models.Eddn
{
    public class Header
    {
        public string softwareVersion { get; set; }
        public DateTime gatewayTimestamp { get; set; }
        public string softwareName { get; set; }
        public string uploaderID { get; set; }
    }
}
