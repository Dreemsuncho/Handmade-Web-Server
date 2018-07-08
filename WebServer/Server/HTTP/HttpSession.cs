using System.Collections.Generic;
using WebServer.Server.Common;
using WebServer.Server.HTTP.Contracts;

namespace WebServer.Server.HTTP
{
    public class HttpSession : IHttpSession
    {
        private readonly string _id;
        private readonly IDictionary<string, object> _parameters;


        public HttpSession(string id)
        {
            CoreValidator.ThrowIfNullOrEmpty(id, nameof(id));

            _id = id;
            _parameters = new Dictionary<string, object>();
        }


        public string Id => _id;


        public void Add(string key, object value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNull(value, nameof(value));

            _parameters.Add(key, value);
        }


        public void Clear()
        {
            _parameters.Clear();
        }


        public object Get(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            object result = null;

            if (_parameters.ContainsKey(key))
                result = _parameters[key];

            return result;
        }


        public T Get<T>(string key)
        {
            return (T)Get(key);
        }


        public bool IsAuthenticated()
        {
            // TODO
            throw new System.NotImplementedException();
        }


        public bool Contains(string key)
        {
            return _parameters.ContainsKey(key);
        }
    }
}
