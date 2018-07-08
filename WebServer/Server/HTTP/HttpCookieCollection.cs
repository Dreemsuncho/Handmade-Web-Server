using System;
using System.Collections;
using System.Collections.Generic;
using WebServer.Server.Common;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.HTTP
{
    class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> _cookies;

        public HttpCookieCollection()
        {
            _cookies = new Dictionary<string, HttpCookie>();
        }


        public void Add(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));

            _cookies.Add(cookie.Key, cookie);
        }


        public void Add(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            Add(new HttpCookie(key, value));
        }


        public bool ContainsKey(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            return _cookies.ContainsKey(key);
        }


        public HttpCookie Get(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            if (!_cookies.ContainsKey(key))
                throw new InvalidOperationException($"The given key '{key}' is not present in the cookies collection.");
            
            return _cookies[key];
        }


        public IEnumerator<HttpCookie> GetEnumerator()
        {
            return _cookies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cookies.Values.GetEnumerator();
        }
    }
}
