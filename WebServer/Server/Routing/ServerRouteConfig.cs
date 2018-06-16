using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebServer.Server.Enums;
using WebServer.Server.Handlers;
using WebServer.Server.Routing.Contracts;

namespace WebServer.Server.Routing
{
    class ServerRouteConfig : IServerRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> _routes;


        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            _routes = new Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>>();

            var reqMethods = Enum
              .GetValues(typeof(HttpRequestMethod))
              .Cast<HttpRequestMethod>();

            foreach (var reqMethod in reqMethods)
            {
                Routes.Add(reqMethod, new Dictionary<string, IRoutingContext>());
            }

            _InitializeServerConfig(appRouteConfig);
        }


        public Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> Routes => _routes;


        private void _InitializeServerConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (KeyValuePair<HttpRequestMethod, Dictionary<string, RequestHandler>> kvp in appRouteConfig.Routes)
            {
                foreach (KeyValuePair<string, RequestHandler> reqHandler in kvp.Value)
                {
                    var parameters = new List<string>();

                    string parsedRegex = _ParseRoute(reqHandler.Key, parameters);

                    var routingContext = new RoutingContext(reqHandler.Value, parameters);
                    Routes[kvp.Key].Add(parsedRegex, routingContext);
                }
            }

        }

        private string _ParseRoute(string reqHandlerKey, List<string> parameters)
        {
            var parsedRegex = new StringBuilder();

            parsedRegex.Append("^");

            if (reqHandlerKey == "/")
            {
                parsedRegex.Append($"{reqHandlerKey}$");
                return parsedRegex.ToString();
            }

            string[] tokens = reqHandlerKey.Split("/");

            _ParseTokens(parameters, tokens, parsedRegex);
            return parsedRegex.ToString();

        }


        private void _ParseTokens(List<string> parameters, string[] tokens, StringBuilder parsedRegex)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                string currentToken = tokens[i];

                string end = (i == tokens.Length - 1)
                    ? "$" : "/";

                if (!currentToken.StartsWith("{") &&
                    !currentToken.EndsWith("}"))
                {
                    parsedRegex.Append($"{currentToken}{end}");
                    continue;
                }

                var regex = new Regex("<\\w+>");
                var match = regex.Match(currentToken);

                if (!match.Success)
                    throw new InvalidOperationException($"Route parameter in '{currentToken}' is not valid.");


                string paramName = match.Value
                    .Substring(1, match.Value.Length - 2);

                parameters.Add(paramName);

                string currentTokenWithoutBrackets = currentToken
                    .Substring(1, currentToken.Length - 2);

                parsedRegex.Append($"{currentTokenWithoutBrackets}{end}");
            }
        }

    }
}
