using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebServer.Server.Handlers.Contracts;
using WebServer.Server.HTTP.Contracts;
using WebServer.Server.HTTP.Response;
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
            var routes = _serverRouteConfig.Routes[httpContext.Request.RequestMethod];
            var request = httpContext.Request;

            try
            {
                foreach (var kvp in routes)
                {
                    var routingPattern = kvp.Key;
                    var routingContext = kvp.Value;

                    var regex = new Regex(routingPattern);
                    var match = regex.Match(request.Path);

                    if (!match.Success) continue;

                    routingContext.Parameters.ToList().ForEach(param =>
                        request.AddUrlParameters(param, match.Groups[param].Value)
                    );

                    return routingContext.RequestHandler.Handle(httpContext);
                }
            }
            catch (Exception ex)
            {
                return new InternalServerErrorResponse(ex);
            }

            return new NotFoundResponse();
        }
    }
}
