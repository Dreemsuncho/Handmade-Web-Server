using WebServer.Application.Views;
using WebServer.Server.Enums;
using WebServer.Server.HTTP.Contracts;
using WebServer.Server.HTTP.Response;

namespace WebServer.Application.Controllers
{
    class HomeController
    {
        public IHttpResponse Index()
        {
            return new ViewResponse(HttpStatusCode.OK, new HomeIndexView());
        }
    }
}
