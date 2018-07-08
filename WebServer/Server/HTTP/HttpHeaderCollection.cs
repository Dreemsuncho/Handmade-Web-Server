using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WebServer.Server.Common;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.HTTP
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, ICollection<HttpHeader>> _headers;


        public HttpHeaderCollection()
        {
            _headers = new Dictionary<string, ICollection<HttpHeader>>();
        }


        public void Add(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));

            string headerKey = header.Key;

            if (!_headers.ContainsKey(headerKey))
                _headers.Add(headerKey, new List<HttpHeader>());

            _headers[headerKey].Add(header);
        }


        public void Add(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            Add(new HttpHeader(key, value));
        }


        public bool ContainsKey(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            return _headers.ContainsKey(key);
        }


        public ICollection<HttpHeader> Get(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            if (!_headers.ContainsKey(key))
                throw new InvalidOperationException($"The given key {key} is not present in the headers collection.");

            return _headers[key];
        }


        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (var header in _headers)
                foreach (var headerValue in header.Value)
                    result.AppendLine($"{header.Key}: {headerValue.Value}");

            return result.ToString();
        }


        public IEnumerator<ICollection<HttpHeader>> GetEnumerator()
            => _headers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _headers.Values.GetEnumerator();
    }
}
