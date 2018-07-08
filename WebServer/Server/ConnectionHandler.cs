using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Server.Common;
using WebServer.Server.Handlers;
using WebServer.Server.HTTP;
using WebServer.Server.HTTP.Contracts;
using WebServer.Server.Routing.Contracts;


namespace WebServer.Server
{
    public class ConnectionHandler
    {
        private readonly Socket _client;
        private readonly IServerRouteConfig _serverRouteConfig;


        public ConnectionHandler(Socket client, IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            _client = client;
            _serverRouteConfig = serverRouteConfig;
        }


        public async Task ProcessRequestAsync()
        {
            string request = await _ReadRequest();

            if (request != string.Empty)
            {
                var httpContext = new HttpContext(request);
                var httpHandler = new HttpHandler(_serverRouteConfig);

                IHttpResponse response = httpHandler.Handle(httpContext);

                var byteSegments = new ArraySegment<byte>(Encoding.ASCII.GetBytes(response.Response));

                await _client.SendAsync(byteSegments, SocketFlags.None);

                Console.WriteLine($"-----REQUEST-----");
                Console.WriteLine(request);
                Console.WriteLine($"-----RESPONSE-----");
                Console.WriteLine(response);
                Console.WriteLine();
            }

            _client.Shutdown(SocketShutdown.Both);
        }


        private async Task<string> _ReadRequest()
        {
            var request = new StringBuilder();
            var receivedData = new ArraySegment<byte>(new byte[1024]);

            int numBytesRead = 0;

            while ((numBytesRead = await _client.ReceiveAsync(receivedData.Array, SocketFlags.None)) > 0)
            {
                request.Append(Encoding.ASCII.GetString(receivedData.Array, 0, numBytesRead));

                if (numBytesRead < 1023)
                    break;
            }

            return request.ToString();
        }
    }
}
