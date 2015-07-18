using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eddi.LoaderService.Modules;
using Topshelf;
using Topshelf.Ninject;

namespace Eddi.LoaderService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(host =>
            {
                host.UseNinject(new QuartzModule(),
                                new PersistenceModule(),
                                new CancellationModule(),
                                new EddnModule())
                    .Service<HostService>(service =>
                    {
                        service.ConstructUsingNinject()
                            .WhenStarted(svc => svc.Start())
                            .WhenStopped(svc => svc.Stop());
                    });
            });
        }
    }
}
