using System;
using WebServer.Server.Common;
using WebServer.Server.Enums;


namespace WebServer.Server.HTTP.Response
{
    public class InternalServerErrorResponse : ViewResponse
    {
        public InternalServerErrorResponse(Exception ex)
            : base(HttpStatusCode.InternalServerError, new InternalServerErrorView(ex))
        {
        }
    }
}
