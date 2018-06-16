using System;
using System.Collections.Generic;
using System.Net;
using WebServer.Server.Enums;
using WebServer.Server.Exceptions;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.HTTP
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            HeaderCollection = new HttpHeaderCollection();
            UrlParameters = new Dictionary<string, string>();
            QueryParameters = new Dictionary<string, string>();
            FormData = new Dictionary<string, string>();

            _ParseRequest(requestString);
        }


        public Dictionary<string, string> FormData { get; }

        public HttpHeaderCollection HeaderCollection { get; }

        public string Path { get; set; }

        public Dictionary<string, string> QueryParameters { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, string> UrlParameters { get; }

        public void AddUrlParameters(string key, string value)
        {
            UrlParameters.Add(key, value);
        }

        private void _ParseRequest(string requestString)
        {
            string[] requestLines = requestString.Split(Environment.NewLine);

            string[] requestLine = requestLines[0].Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 ||
                requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid request line");
            }

            RequestMethod = _ParseRequestMethod(requestLine[0].ToUpper());
            Url = requestLine[1];
            Path = Url.Split("?#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];

            _ParseHeaders(requestLines);
            _ParseParameters();

            if (RequestMethod == HttpRequestMethod.POST)
                _ParseQuery(requestLines[requestLines.Length - 1], FormData);
        }

        private void _ParseParameters()
        {
            if (!Url.Contains("?"))
                return;

            string query = Url.Split("?")[1];
            _ParseQuery(query, QueryParameters);
        }

        private void _ParseQuery(string query, Dictionary<string, string> queryParameters)
        {
            if (!query.Contains("="))
                return;

            string[] queryPairs = query.Split('&');
            foreach (var qp in queryPairs)
            {
                var queryArgs = qp.Split('=');
                if (queryArgs.Length != 2)
                    continue;

                queryParameters.Add(
                    WebUtility.UrlDecode(queryArgs[0]),
                    WebUtility.UrlDecode(queryArgs[1]));
            }
        }

        private void _ParseHeaders(string[] requestLines)
        {
            int endIndex = Array.IndexOf(requestLines, string.Empty);
            var hasHostHeader = false;

            for (int i = 1; i < endIndex; i++)
            {
                string[] headerArgs = requestLines[i].Split(": ");

                string key = headerArgs[0];
                string value = headerArgs[1];

                HeaderCollection.Add(new HttpHeader(key, value));

                if (key.ToLower() == "host")
                    hasHostHeader = true;
            }

            if (!hasHostHeader)
                throw new BadRequestException("Missing host header");
        }

        private HttpRequestMethod _ParseRequestMethod(string requestMethod)
        {
            switch (requestMethod)
            {
                case "GET":
                    return HttpRequestMethod.GET;
                case "POST":
                    return HttpRequestMethod.GET;
                case "PUT":
                    return HttpRequestMethod.PUT;
                case "DELETE":
                    return HttpRequestMethod.DELETE;

                default:
                    throw new BadRequestException("Cannot recognize request method");
            }
        }
    }
}
