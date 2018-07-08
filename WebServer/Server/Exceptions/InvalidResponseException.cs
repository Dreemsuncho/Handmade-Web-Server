using System;

namespace WebServer.Server.Exceptions
{
    class InvalidResponseException : Exception
    {
        public InvalidResponseException(string message) : base(message)
        {

        }
    }
}
