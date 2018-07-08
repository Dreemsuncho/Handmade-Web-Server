using System;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.Handlers
{
    public class PostHandler : RequestHandler
    {
        public PostHandler(Func<IHttpRequest, IHttpResponse> f)
            : base(f)
        {
        }
    }
}
