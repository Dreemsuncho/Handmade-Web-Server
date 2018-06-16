using System;
using WebServer.Server.Handlers.Contracts;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.Handlers
{
    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpResponse> _f;


        public RequestHandler(Func<IHttpResponse> f)
        {
            _f = f;
        }


        public IHttpResponse Handle(IHttpContext httpContext)
        {
            IHttpResponse httpResponse = _f.Invoke();
            httpResponse.AddHeader("content-type", "text/html");
            return httpResponse;
        }
    }
}
