using Newtonsoft.Json;
using System;

namespace Kraken.Model
{
    public class OptimizeSetUploadRequest : OptimizeSetRequestBase, IOptimizeSetUploadRequest
    {
        public OptimizeSetUploadRequest(Uri callbackUrl)
        {
            CallbackUrl = callbackUrl;
        }

        [JsonProperty("callback_url")]
        public Uri CallbackUrl { get; set; }
    }
}
