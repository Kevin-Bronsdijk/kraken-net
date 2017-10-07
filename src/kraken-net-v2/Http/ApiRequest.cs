using System.Net.Http;
using Kraken.Model;

namespace Kraken.Http
{
    internal class ApiRequest : IApiRequest
    {
        public ApiRequest(IRequest body, string uri)
        {
            Uri = uri;
            Method = HttpMethod.Post;
            Body = body;
        }

        public string Uri { get; set; }
        public HttpMethod Method { get; set; }
        public IRequest Body { get; set; }
    }
}