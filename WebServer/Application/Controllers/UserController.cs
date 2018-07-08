using System.Collections.Generic;
using WebServer.Application.Views;
using WebServer.Server;
using WebServer.Server.Enums;
using WebServer.Server.HTTP.Contracts;
using WebServer.Server.HTTP.Response;

namespace WebServer.Application.Controllers
{
    class UserController
    {
        public IHttpResponse RegisterGet()
        {
            return new ViewResponse(HttpStatusCode.OK, new RegisterView());
        }


        public IHttpResponse RegisterPost(string name)
        {
            return new RedirectResponse($"/user/{name}");
        }


        public IHttpResponse Details(string name)
        {
            var model = new Model { [name] = "name" };
            return new ViewResponse(HttpStatusCode.OK, new UserDetailsView(model));
        }
    }
}
