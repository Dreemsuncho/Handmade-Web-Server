using System;
using WebServer.Server.Common;

namespace WebServer.Server.HTTP
{
    public class HttpCookie
    {
        private readonly string _key;
        private readonly string _value;
        private readonly DateTime _expires;
        private readonly bool _isNew;


        public HttpCookie(string key, string value, int expires = 3 /* in days */)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            _key = key;
            _value = value;
            _expires = DateTime.UtcNow.AddDays(expires);
        }

        public HttpCookie(string key, string value, bool isNew, int expires = 3) 
            : this(key, value, expires)
        {
            _isNew = isNew;
        }


        public string Key => _key;
        public string Value => _value;
        public DateTime Expires => _expires;
        public bool IsNew => _isNew;


        public override string ToString()
        {
            return $"{Key}={Value}; Expires={Expires.ToLongTimeString()}";
        }
    }
}
