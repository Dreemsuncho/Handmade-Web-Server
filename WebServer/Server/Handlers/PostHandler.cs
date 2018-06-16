using System;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.Handlers
{
    public class PostHandler : RequestHandler
    {
        public PostHandler(Func<IHttpResponse> f)
            : base(f)
        {
        }
    }
}
