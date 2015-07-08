using Topshelf;

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
                x.Service<EddbLoaderService>(s =>
                {
                    s.ConstructUsing(name => new EddbLoaderService());
                    s.WhenStarted(el => el.Start());
                    s.WhenStopped(el => el.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Eddb Loader Service.");
                x.SetDisplayName("EddbLoader");
                x.SetServiceName("EddbLoader");
            });
        }
    }
}
