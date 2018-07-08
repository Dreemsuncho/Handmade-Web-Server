using WebServer.Application.Controllers;
using WebServer.Server.Contracts;
using WebServer.Server.Handlers;
using WebServer.Server.Routing.Contracts;

namespace WebServer.Application
{
    public class MainApplication : IApplication
    {
        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.AddRoute("/",
                new GetHandler(request => new HomeController().Index()));

            appRouteConfig.AddRoute("/register",
                new GetHandler(request => new UserController().RegisterGet()));

            appRouteConfig.AddRoute("/register",
                new PostHandler(request => new UserController().RegisterPost(request.FormData["name"])));

            appRouteConfig.AddRoute("/user/{(?<name>[a-z]+)}",
                new GetHandler(request => new UserController().Details(request.UrlParameters["name"])));

        }
    }
}
