using System;
using Newtonsoft.Json;

namespace Kraken.Model.S3
{
    public class OptimizeSetUploadRequest : Model.OptimizeSetUploadRequest
    {
        public OptimizeSetUploadRequest(Uri callbackUrl, DataStore dataStore) : base(callbackUrl)
        {
            S3Store = dataStore;
        }

        [JsonProperty("s3_store")]
        public DataStore S3Store { get; internal set; }
    }
}
