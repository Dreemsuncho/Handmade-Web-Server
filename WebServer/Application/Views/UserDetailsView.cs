using WebServer.Server;
using WebServer.Server.Contracts;

namespace WebServer.Application.Views
{
    internal class UserDetailsView : IView
    {
        private readonly Model _model;

        public UserDetailsView(Model model)
        {
            _model = model;
        }


        public string View()
        {
            return $"<body>Hello, {_model["name"]}!</body>";
        }
    }
}