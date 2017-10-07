using System;
using Newtonsoft.Json;

namespace Kraken.Model.Azure
{
    public class OptimizeSetWaitRequest : Model.OptimizeSetWaitRequest
    {
        public OptimizeSetWaitRequest(Uri imageUrl, DataStore dataStore) : base(imageUrl)
        {
            BlobStore = dataStore;
        }

        [JsonProperty("azure_store")]
        public DataStore BlobStore { get; internal set; }
    }
}
