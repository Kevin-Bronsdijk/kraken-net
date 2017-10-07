using System;
using Newtonsoft.Json;

namespace Kraken.Model
{
    public class OptimizeSetRequest : OptimizeSetRequestBase, IOptimizeSetRequest
    {
        public OptimizeSetRequest(Uri imageUrl, Uri callbackUrl)
        {
            ImageUrl = imageUrl;
            CallbackUrl = callbackUrl;
        }

        [JsonProperty("url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("callback_url")]
        public Uri CallbackUrl { get; set; }
    }
}
