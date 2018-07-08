using System.Collections.Generic;

namespace WebServer.Server
{

    public class Model
    {
        private readonly Dictionary<string, object> _objects;


        public Model()
        {
            _objects = new Dictionary<string, object>();
        }


        public object this[string key]
        {
            get => _objects[key];
            set => _objects[key] = value;
        }

        public void Add(string key, object value)
        {
            _objects.Add(key, value);
        }
    }
}
