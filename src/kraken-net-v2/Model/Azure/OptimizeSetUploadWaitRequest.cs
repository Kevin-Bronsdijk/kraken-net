using System;
using Newtonsoft.Json;

namespace Kraken.Model.Azure
{
    public class OptimizeSetUploadWaitRequest : Model.OptimizeSetUploadWaitRequest
    {
        public OptimizeSetUploadWaitRequest(DataStore dataStore)
        {
            BlobStore = dataStore;
        }

        [JsonProperty("azure_store")]
        public DataStore BlobStore { get; internal set; }
    }
}
