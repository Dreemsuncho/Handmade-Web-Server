using System;
using System.Collections.Generic;
using System.Linq;
using WebServer.Server.Enums;
using WebServer.Server.Handlers;
using WebServer.Server.Routing.Contracts;

namespace WebServer.Server.Routing
{
    public class AppRouteConfig : IAppRouteConfig
    {
        private Dictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> _routes;


        public AppRouteConfig()
        {
            _routes = new Dictionary<HttpRequestMethod, Dictionary<string, RequestHandler>>();

            var reqMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (HttpRequestMethod reqMethod in reqMethods)
            {
                _routes.Add(reqMethod, new Dictionary<string, RequestHandler>());
            }
        }


        public IReadOnlyDictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> Routes => _routes;


        public void AddRoute(string route, RequestHandler httpHandler)
        {
            string requestHandlerName = httpHandler
                .GetType().ToString().ToLower();

            if (requestHandlerName.Contains("get"))
            {
                Routes[HttpRequestMethod.GET].Add(route, httpHandler);
            }
            else if (requestHandlerName.Contains("post"))
            {
                Routes[HttpRequestMethod.POST].Add(route, httpHandler);
            }
        }
    }
}
