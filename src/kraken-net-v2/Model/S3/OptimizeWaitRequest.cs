using System;
using Newtonsoft.Json;

namespace Kraken.Model.S3
{
    public class OptimizeWaitRequest : Model.OptimizeWaitRequest
    {
        public OptimizeWaitRequest(Uri imageUrl, DataStore dataStore) : base(imageUrl)
        {
            S3Store = dataStore;
        }

        public OptimizeWaitRequest(Uri imageUrl, string key, string secret, string bucket, string region)
            : base(imageUrl)
        {
            S3Store = new DataStore(key, secret, bucket, region);
        }

        [JsonProperty("s3_store")]
        public DataStore S3Store { get; internal set; }
    }
}