using WebServer.Application;
using WebServer.Server.Contracts;
using WebServer.Server.Routing;


namespace WebServer
{
    class Launcher : IRunnable
    {
        private Server.WebServer _webServer;


        public static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            var app = new MainApplication();
            var routeConfig = new AppRouteConfig();
            app.Start(routeConfig);

            _webServer = new Server.WebServer(port: 8230, appRouteConfig: routeConfig);

            _webServer.Run();
        }
    }
}
