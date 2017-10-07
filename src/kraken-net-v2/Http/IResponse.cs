using System.Net;

namespace Kraken.Http
{
    public interface IApiResponse
    {
        bool Success { get; }
        HttpStatusCode StatusCode { get; }
        string Error { get; }
    }
}