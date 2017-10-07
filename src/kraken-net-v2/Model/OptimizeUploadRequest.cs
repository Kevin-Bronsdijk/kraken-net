using System;
using Newtonsoft.Json;

namespace Kraken.Model
{
    public class OptimizeUploadRequest : OptimizeRequestBase, IOptimizeUploadRequest
    {
        public OptimizeUploadRequest(Uri callbackUrl)
        {
            CallbackUrl = callbackUrl;
        }

        [JsonProperty("callback_url")]
        public Uri CallbackUrl { get; set; }
    }
}