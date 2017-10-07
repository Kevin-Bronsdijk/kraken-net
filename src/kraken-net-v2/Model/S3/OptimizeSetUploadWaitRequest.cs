using Newtonsoft.Json;

namespace Kraken.Model.S3
{
    public class OptimizeSetUploadWaitRequest : Model.OptimizeSetUploadWaitRequest
    {
        public OptimizeSetUploadWaitRequest(DataStore dataStore)
        {
            S3Store = dataStore;
        }

        [JsonProperty("s3_store")]
        public DataStore S3Store { get; internal set; }
    }
}
