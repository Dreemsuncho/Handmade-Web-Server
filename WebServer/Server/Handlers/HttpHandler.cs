using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebServer.Server.Handlers.Contracts;
using WebServer.Server.HTTP.Contracts;
using WebServer.Server.Routing.Contracts;

namespace WebServer.Server.Handlers
{
    class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig _serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            _serverRouteConfig = serverRouteConfig;
        }


        public IHttpResponse Handle(IHttpContext httpContext)
        {

            Dictionary<string, IRoutingContext> routes = _serverRouteConfig.Routes[httpContext.Request.RequestMethod];

            foreach (var kvp in routes)
            {
                IRoutingContext routingContext = kvp.Value;
                IHttpRequest httpRequest = httpContext.Request;

                var routingPattern = kvp.Key;
                var regex = new Regex(routingPattern);
                var match = regex.Match(httpRequest.Path);

                if (!match.Success)
                    continue;

                foreach (string param in routingContext.Parameters)
                    httpRequest.AddUrlParameters(param, match.Groups[param].Value);

                return routingContext.RequestHandler.Handle(httpContext);
            }

            throw new NotImplementedException();
        }
    }
}
