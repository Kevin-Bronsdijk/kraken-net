using System;
using Newtonsoft.Json;

namespace Kraken.Model.Azure
{
    public class OptimizeSetRequest : Model.OptimizeSetRequest
    {
        public OptimizeSetRequest(Uri imageUrl, Uri callbackUrl, DataStore dataStore) : base(imageUrl, callbackUrl)
        {
            BlobStore = dataStore;
        }

        [JsonProperty("azure_store")]
        public DataStore BlobStore { get; internal set; }
    }
}
