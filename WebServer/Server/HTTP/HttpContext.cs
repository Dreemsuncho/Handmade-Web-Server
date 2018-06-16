using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.HTTP
{
    class HttpContext : IHttpContext
    {
        private readonly IHttpRequest _request;

        public HttpContext(string requestString)
        {
            _request = new HttpRequest(requestString);
        }

        public IHttpRequest Request => _request;
    }
}
