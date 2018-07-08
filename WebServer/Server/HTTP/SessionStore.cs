using System.Collections.Concurrent;

namespace WebServer.Server.HTTP
{
    public static class SessionStore
    {
        public const string SessionCookieKey = "MY_SID";
        private static readonly ConcurrentDictionary<string, HttpSession> _sessions;


        static SessionStore()
        {
             _sessions = new ConcurrentDictionary<string, HttpSession>();
        }


        public static HttpSession GetOrAdd(string id) => 
            _sessions.GetOrAdd(id, _ => new HttpSession(id));
    }
}
