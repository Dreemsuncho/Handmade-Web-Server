using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebServer.Server.Common;
using WebServer.Server.Enums;
using WebServer.Server.Exceptions;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.HTTP
{
    public class HttpRequest : IHttpRequest
    {
        private readonly string _requestText;

        public HttpRequest(string requestString)
        {
            Headers = new HttpHeaderCollection();
            UrlParameters = new Dictionary<string, string>();
            QueryParameters = new Dictionary<string, string>();
            FormData = new Dictionary<string, string>();
            Cookies = new HttpCookieCollection();

            _requestText = requestString;

            _ParseRequest(requestString);
        }


        public IDictionary<string, string> FormData { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public IDictionary<string, string> QueryParameters { get; }

        public IDictionary<string, string> UrlParameters { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpSession Session { get; set; }

        public string Path { get; private set; }

        public string Url { get; private set; }


        public void AddUrlParameters(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            UrlParameters.Add(key, value);
        }


        private void _ParseRequest(string requestString)
        {
            string[] requestLines = requestString.Split(Environment.NewLine);

            if (!requestLines.Any())
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            string[] requestLine = requestLines[0].Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            RequestMethod = _ParseRequestMethod(requestLine[0].ToUpper());
            Url = requestLine[1];
            Path = Url.Split("?#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];

            _ParseHeaders(requestLines);
            _ParseCookies();
            _ParseParameters();

            if (RequestMethod == HttpRequestMethod.POST)
                _ParseQuery(requestLines.Last(), FormData);

            _SetSession();
        }


        private void _ParseCookies()
        {
            if (Headers.ContainsKey(HttpHeader.Cookie))
            {
                var cookies = Headers.Get(HttpHeader.Cookie);

                foreach (var cookie in cookies)
                {
                    if (!cookie.Value.Contains('='))
                        return;

                    var cookieArgs = cookie.Value
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    foreach (var cookieArg in cookieArgs)
                    {
                        var cookieKeyValuePair = cookieArg.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                        if (cookieKeyValuePair.Length == 2)
                        {
                            var key = cookieKeyValuePair[0].Trim();
                            var value = cookieKeyValuePair[1].Trim();

                            Cookies.Add(new HttpCookie(key, value, isNew: false));
                        }
                    }
                }
            }
        }


        private void _ParseParameters()
        {
            if (!Url.Contains("?"))
                return;

            string query = Url.Split("?")[1];
            _ParseQuery(query, QueryParameters);
        }


        private void _ParseQuery(string query, IDictionary<string, string> queryParameters)
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

                if (headerArgs.Length != 2)
                    BadRequestException.ThrowFromInvalidRequest();

                string key = headerArgs[0];
                string value = headerArgs[1];

                Headers.Add(new HttpHeader(key, value));

                if (key.ToLower() == HttpHeader.Host.ToLower())
                    hasHostHeader = true;
            }


            if (!hasHostHeader)
                BadRequestException.ThrowFromInvalidRequest();
        }


        private HttpRequestMethod _ParseRequestMethod(string requestMethod)
        {
            switch (requestMethod)
            {
                case "GET":
                    return HttpRequestMethod.GET;
                case "POST":
                    return HttpRequestMethod.POST;
                case "PUT":
                    return HttpRequestMethod.PUT;
                case "DELETE":
                    return HttpRequestMethod.DELETE;

                default:
                    throw new BadRequestException("Cannot recognize request method");
            }
        }


        private void _SetSession()
        {
            if (Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                var cookie = Cookies.Get(SessionStore.SessionCookieKey);
                var sessionId = cookie.Value;

                Session = SessionStore.GetOrAdd(id: sessionId);
            }
        }


        public override string ToString() => _requestText;
    }
}
