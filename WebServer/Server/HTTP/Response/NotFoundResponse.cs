using WebServer.Server.Common;
using WebServer.Server.Enums;

namespace WebServer.Server.HTTP.Response
{
    class NotFoundResponse : ViewResponse
    {
        public NotFoundResponse()
            : base(HttpStatusCode.NotFound, new NotFoundView())
        {
        }
    }
}
