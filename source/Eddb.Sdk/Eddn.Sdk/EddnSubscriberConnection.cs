using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zlib;
using ZeroMQ;

namespace Eddn.Sdk
{
    public class EddnSubscriber : IEddnSubscriber
    {
        internal EddnSubscriber()
        {
        }

        public static IEddnSubscriber Create()
        {
            return new EddnSubscriber();
        }


        protected async Task<string> ReceiveMessage(ZSocket subscriber, CancellationToken cancellationToken)
        {
            var frame = await Task.Run(() =>
            {
                try
                {
                    return subscriber.ReceiveFrame();
                }
                catch (ZException zex)
                {
                    if (zex.ErrNo == 11) //Exceeded Timeout
                        return null;

                    throw;
                }
            }, cancellationToken);
            if (frame != null)
            {
                using (var ms = new MemoryStream())
                {
                    frame.CopyTo(ms);

                    ms.Position = 0;

                    using (ZlibStream stream = new ZlibStream(ms, CompressionMode.Decompress))
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        var json = sr.ReadToEnd();

                        return json;
                    }
                }
            }

            return null;
        }

        public Task BeginListener(Action<string> callback, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(async () =>
            {
                using (var ztx = new ZContext())
                using (var subscriber = new ZSocket(ztx, ZSocketType.SUB))
                {
                    subscriber.Connect("tcp://eddn-relay.elite-markets.net:9500");
                    subscriber.Subscribe("");
                    subscriber.ReceiveTimeout = new TimeSpan(0, 0, 30);

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var message = await ReceiveMessage(subscriber, cancellationToken);

                        if (message != null)
                            Task.Run(() => callback(message), cancellationToken).Start();
                    }
                }
            }, cancellationToken);
        }
    }
}
