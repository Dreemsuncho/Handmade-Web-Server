using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using WebServer.Server.Contracts;
using WebServer.Server.Routing;
using WebServer.Server.Routing.Contracts;

namespace WebServer.Server
{
    public class WebServer : IRunnable
    {
        private readonly int _port;
        private readonly TcpListener _tcpListener;
        private readonly IServerRouteConfig _serverRouteConfig;
        private bool _isRunning;

        public WebServer(int port, IAppRouteConfig appRouteConfig)
        {
            _port = port;
            _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), _port);

            _serverRouteConfig = new ServerRouteConfig(appRouteConfig);
        }

        public void Run()
        {
            _tcpListener.Start();
            _isRunning = true;

            Console.WriteLine($"Server started listening on TCP clients at 127.0.0.1:{_port}");

            var task = Task.Run(_ListenLoop);
        }

        private async Task _ListenLoop()
        {
            while (_isRunning)
            {
                Socket client = await _tcpListener.AcceptSocketAsync();

                var connectionHandler = new ConnectionHandler(client, _serverRouteConfig);
                Task connection = connectionHandler.ProcessRequestAsync();
                connection.Wait();
            }
        }
    }
}
