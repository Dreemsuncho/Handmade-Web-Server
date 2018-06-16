using System;

namespace WebServer.Server.Exceptions
{
    class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message) { }
    }
}
