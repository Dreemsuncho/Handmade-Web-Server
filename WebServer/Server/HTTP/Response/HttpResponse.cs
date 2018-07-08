using System.Text;
using WebServer.Server.Contracts;
using WebServer.Server.Enums;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.HTTP.Response
{
    public abstract class HttpResponse : IHttpResponse
    {
        private readonly IView _view;

        private HttpStatusCode _statusCode = HttpStatusCode.Found;

        private readonly HttpHeaderCollection _headerCollection = new HttpHeaderCollection();

        private readonly HttpCookieCollection _cookieCollection = new HttpCookieCollection();


        public HttpResponse(string redirectUrl)
        {
            AddHeader("Location", redirectUrl);
        }

        public HttpResponse(HttpStatusCode responseCode, IView view)
        {
            _statusCode = responseCode;
            _view = view;
        }


        private string _StatusMessage => _statusCode.ToString();

        public string Response
        {
            get
            {
                var result = new StringBuilder();

                result.AppendLine($"HTTP/1.1 {_statusCode} {_StatusMessage}");
                result.AppendLine(_headerCollection.ToString());
                result.AppendLine();

                if ((int)_statusCode < 300 || (int)_statusCode > 400)
                {
                    result.AppendLine(_view.View());
                }

                return result.ToString();
            }
        }


        public void AddHeader(string key, string value)
        {
            _headerCollection.Add(new HttpHeader(key, value));
        }


        public bool ContainsHeader(string key)
        {
            return _headerCollection.ContainsKey(key);
        }


        public void SetCookies()
        {
            foreach (var c in _cookieCollection)
                AddHeader(HttpHeader.SetCookie, c.ToString());
        }
    }
}
