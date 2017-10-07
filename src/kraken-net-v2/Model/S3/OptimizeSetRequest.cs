using System;
using Newtonsoft.Json;

namespace Kraken.Model.S3
{
    public class OptimizeSetRequest : Model.OptimizeSetRequest
    {
        public OptimizeSetRequest(Uri imageUrl, Uri callbackUrl, DataStore dataStore) : base(imageUrl, callbackUrl)
        {
            S3Store = dataStore;
        }

        [JsonProperty("s3_store")]
        public DataStore S3Store { get; internal set; }
    }
}
