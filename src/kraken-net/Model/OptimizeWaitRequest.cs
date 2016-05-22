using System;
using Newtonsoft.Json;

namespace Kraken.Model
{
    public class OptimizeWaitRequest : OptimizeRequestBase, IOptimizeWaitRequest
    {
        public OptimizeWaitRequest(Uri imageUrl)
        {
            ImageUrl = imageUrl;
        }

        [JsonProperty("url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("wait")]
        internal bool Wait { get; } = true;
    }
}