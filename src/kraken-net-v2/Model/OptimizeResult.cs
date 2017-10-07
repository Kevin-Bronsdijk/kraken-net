using Newtonsoft.Json;

namespace Kraken.Model
{
    public class OptimizeResult
    {
        internal OptimizeResult()
        {
        }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}