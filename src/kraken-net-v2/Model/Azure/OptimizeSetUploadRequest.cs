using System;
using Newtonsoft.Json;

namespace Kraken.Model.Azure
{
    public class OptimizeSetUploadRequest : Model.OptimizeSetUploadRequest
    {
        public OptimizeSetUploadRequest(Uri callbackUrl, DataStore dataStore) : base(callbackUrl)
        {
            BlobStore = dataStore;
        }

        [JsonProperty("azure_store")]
        public DataStore BlobStore { get; internal set; }
    }
}
