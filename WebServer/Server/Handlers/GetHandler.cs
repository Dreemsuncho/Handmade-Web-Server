using System;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.Handlers
{
    public class GetHandler : RequestHandler
    {
        public GetHandler(Func<IHttpRequest, IHttpResponse> f) 
            : base(f)
        {
        }
    }
}
