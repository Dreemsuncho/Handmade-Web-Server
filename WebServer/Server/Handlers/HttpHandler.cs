using System;
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
            foreach (var kvp in _serverRouteConfig.Routes[httpContext.Request.RequestMethod])
            {
                string pattern = kvp.Key;
                var regex = new Regex(pattern);
                var match = regex.Match(httpContext.Request.Path);

                if (!match.Success)
                    continue;

                foreach (var param in kvp.Value.Parameters)
                {
                    httpContext.Request.AddUrlParameters(param, match.Groups[param].Value);
                }

                return kvp.Value.RequestHandler.Handle(httpContext);
            }

            throw new NotImplementedException();
        }
    }
}
