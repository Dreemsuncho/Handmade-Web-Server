using System.Collections.Generic;
using WebServer.Server.Common;
using WebServer.Server.Handlers;
using WebServer.Server.Routing.Contracts;

namespace WebServer.Server.Routing
{
    class RoutingContext : IRoutingContext
    {
        public RoutingContext(RequestHandler handler, IEnumerable<string> parameters)
        {
            CoreValidator.ThrowIfNull(handler, nameof(handler));
            CoreValidator.ThrowIfNull(parameters, nameof(parameters));

            RequestHandler = handler;
            Parameters = parameters;
        }


        public RequestHandler RequestHandler { get; }

        public IEnumerable<string> Parameters { get; }
    }
}
