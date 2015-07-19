using Topshelf;
using Topshelf.Ninject;

namespace Eddb.Loader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            HostFactory.Run(x =>
            {
                var hostConfig = x.UseNinject(new LoaderModule());
                
                x.Service<EddbLoaderService>(s =>
                {
                    s.ConstructUsingNinject();
                    s.WhenStarted(el => el.Start());
                    s.WhenStopped(el => el.Stop());
                });
                
                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.SetDescription("Eddb Loader Service.");
                x.SetDisplayName("EddbLoader");
                x.SetServiceName("EddbLoader");
            });
        }
    }
}
