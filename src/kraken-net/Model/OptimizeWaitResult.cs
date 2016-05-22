using Newtonsoft.Json;

namespace Kraken.Model
{
    public class OptimizeWaitResult
    {
        internal OptimizeWaitResult()
        {
        }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("original_size")]
        public int OriginalSize { get; set; }

        [JsonProperty("kraked_size")]
        public int KrakedSize { get; set; }

        [JsonProperty("saved_bytes")]
        public int SavedBytes { get; set; }

        [JsonProperty("kraked_url")]
        public string KrakedUrl { get; set; }
    }
}