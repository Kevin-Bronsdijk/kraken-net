using System.Net.Http;
using Kraken.Model;

namespace Kraken.Http
{
    internal interface IApiRequest
    {
        string Uri { get; set; }
        IRequest Body { get; set; }
        HttpMethod Method { get; set; }
    }
}