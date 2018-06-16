using System.Collections.Generic;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.HTTP
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> _headers;


        public HttpHeaderCollection()
        {
            _headers = new Dictionary<string, HttpHeader>();
        }


        public void Add(HttpHeader header)
        {
            _headers.Add(header.Key, header);
        }

        public bool ContainsKey(string key)
        {
            return _headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            return _headers[key];
        }

        public override string ToString()
        {
            return string.Join("\n", _headers);
        }
    }
}
