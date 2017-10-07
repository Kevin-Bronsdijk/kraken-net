using Newtonsoft.Json;

namespace Kraken.Model
{
    public class ErrorResult
    {
        [JsonProperty("message")]
        public string Error { get; set; }
    }
}