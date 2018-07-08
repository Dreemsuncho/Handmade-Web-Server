using System;

namespace WebServer.Server.Exceptions
{
    class BadRequestException : Exception
    {
        private const string InvalidRequestMessage = "Request is not valid.";

        public BadRequestException(string message) : base(message)
        {

        }

        public static object ThrowFromInvalidRequest()
            => throw new BadRequestException(InvalidRequestMessage);
    }
}
