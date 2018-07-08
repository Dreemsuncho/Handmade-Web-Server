using System;
using WebServer.Server.Common;
using WebServer.Server.Handlers.Contracts;
using WebServer.Server.HTTP;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.Handlers
{
    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> _f;


        public RequestHandler(Func<IHttpRequest, IHttpResponse> f)
        {
            CoreValidator.ThrowIfNull(f, nameof(f));
            _f = f;
        }


        public IHttpResponse Handle(IHttpContext httpContext)
        {
            string sessionIdToSend = null;

            var httpRequest = httpContext.Request;

            if (!httpRequest.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                sessionIdToSend = new Guid().ToString();
                httpRequest.Session = SessionStore.GetOrAdd(sessionIdToSend);
            }

            IHttpResponse httpResponse = _f.Invoke(httpRequest);

            if (sessionIdToSend != null)
            {
                httpResponse.AddHeader(
                    HttpHeader.SetCookie, $"{SessionStore.SessionCookieKey}={sessionIdToSend}; HttpOnly; path=/");
            }

            if (!httpResponse.ContainsHeader(HttpHeader.ContentType))
            {
                httpResponse.AddHeader(HttpHeader.ContentType, "text/plain");
            }

            httpResponse.SetCookies();

            return httpResponse;
        }


        private void _SetCookies(IHttpContext httpContext, IHttpResponse httpResponse)
        {
            var cookies = httpContext.Request.Cookies;

            foreach (HttpCookie c in cookies)
                if (c.IsNew)
                    httpResponse.AddHeader(HttpHeader.SetCookie, c.ToString());
        }
    }
}
