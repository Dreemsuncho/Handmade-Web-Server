using System;
using WebServer.Server.Contracts;

namespace WebServer.Server.Common
{
    public class InternalServerErrorView : IView
    {
        private readonly Exception _exception;


        public InternalServerErrorView(Exception exception)
        {
            _exception = exception;
        }


        public string View()
        {
            return "<h1>" +
                        $"{_exception.Message}" +
                   "</h1>" +

                   "<h2>" +
                       $"{_exception.StackTrace}" +
                   "</h2>";
        }
    }
}
