using WebServer.Server.Contracts;

namespace WebServer.Application.Views
{
    class RegisterView : IView
    {
        public string View()
        {
            return "<body>" +
                        "<form method=\"POST\">" +
                            "Name</br>" +
                            "<input type=\"text\" name=\"name\" /></br>" +
                            "<input type=\"submit\" />" +
                        "</form>" +
                    "</body>";
        }
    }
}
