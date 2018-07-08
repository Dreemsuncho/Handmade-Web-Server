using WebServer.Server.Common;

namespace WebServer.Server.HTTP
{
    public class HttpHeader
    {
        public HttpHeader(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            Key = key;
            Value = value;
        }


        public static string ContentType => "Content-Type";
        public static string Host => "Host";
        public static string Location => "Location";
        public static string Cookie => "Cookie";
        public static string SetCookie => "Set-Cookie";


        public string Key { get; private set; }
        public string Value { get; private set; }


        public override string ToString()
        {
            return Key + ": " + Value;
        }
    }
}
