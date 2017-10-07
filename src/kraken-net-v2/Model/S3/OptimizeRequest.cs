using System;
using Newtonsoft.Json;

namespace Kraken.Model.S3
{
    public class OptimizeRequest : Model.OptimizeRequest
    {
        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, DataStore dataStore) : base(imageUrl, callbackUrl)
        {
            S3Store = dataStore;
        }

        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, string key, string secret, string bucket, string region)
            : base(imageUrl, callbackUrl)
        {
            S3Store = new DataStore(key, secret, bucket, region);
        }

        [JsonProperty("s3_store")]
        public DataStore S3Store { get; internal set; }
    }
}