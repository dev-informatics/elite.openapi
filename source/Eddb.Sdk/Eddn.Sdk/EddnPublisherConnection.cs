using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Eddn.Sdk
{
    public class EddnPublisherConnection
    {
        internal EddnPublisherConnection()
        {
            using (var ztx = new ZContext())
            using (var subscriber = new ZSocket(ztx, ZSocketType.PUB))
            using (var ms = new MemoryStream())
            {
                subscriber.Connect("tcp://eddn-relay.elite-markets.net:9500");

                subscriber.Send(new ZMessage(new[] { new ZFrame(new byte[] { 0x06 }) }));

                subscriber.

                frame.CopyTo(ms);

                ms.Position = 0;

                using (StreamReader sr = new StreamReader(ms))
                {
                    var json = sr.ReadToEnd();

                }
            }
        }
    }
}
