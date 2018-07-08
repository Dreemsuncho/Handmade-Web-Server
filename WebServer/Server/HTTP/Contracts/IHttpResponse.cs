namespace WebServer.Server.HTTP.Contracts
{
    public interface IHttpResponse
    {
        string Response { get; }

        void SetCookies();

        void AddHeader(string key, string value);

        bool ContainsHeader(string key);

    }
}
