using System;
using Newtonsoft.Json;

namespace Kraken.Model.S3
{
    public class OptimizeUploadRequest : Model.OptimizeUploadRequest
    {
        public OptimizeUploadRequest(Uri callbackUrl, DataStore dataStore) : base(callbackUrl)
        {
            S3Store = dataStore;
        }

        public OptimizeUploadRequest(Uri callbackUrl, string key, string secret, string bucket, string region)
            : base(callbackUrl)
        {
            S3Store = new DataStore(key, secret, bucket, region);
        }

        [JsonProperty("s3_store")]
        public DataStore S3Store { get; internal set; }
    }
}