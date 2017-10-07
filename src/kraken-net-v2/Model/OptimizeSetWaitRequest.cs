using System;
using Newtonsoft.Json;

namespace Kraken.Model
{
    public class OptimizeSetWaitRequest : OptimizeSetRequestBase, IOptimizeSetWaitRequest
    {
        public OptimizeSetWaitRequest(Uri imageUrl)
        {
            ImageUrl = imageUrl;
        }

        [JsonProperty("url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("wait")]
        internal bool Wait { get; set; } = true;
    }
}
