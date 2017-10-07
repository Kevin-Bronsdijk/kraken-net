using System.Net;

namespace Kraken.Http
{
    public class ApiResponse<TResult> : IApiResponse<TResult>
    {
        internal ApiResponse()
        {
        }

        public string Error { get; internal set; }
        public TResult Body { get; internal set; }
        public bool Success { get; internal set; }
        public HttpStatusCode StatusCode { get; internal set; }
    }
}