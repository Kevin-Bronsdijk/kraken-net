using Newtonsoft.Json;

namespace Kraken.Model.S3
{
    public class OptimizeUploadWaitRequest : Model.OptimizeUploadWaitRequest
    {
        public OptimizeUploadWaitRequest(DataStore dataStore)
        {
            S3Store = dataStore;
        }

        public OptimizeUploadWaitRequest(string key, string secret, string bucket, string region)
        {
            S3Store = new DataStore(key, secret, bucket, region);
        }

        [JsonProperty("s3_store")]
        public DataStore S3Store { get; internal set; }
    }
}