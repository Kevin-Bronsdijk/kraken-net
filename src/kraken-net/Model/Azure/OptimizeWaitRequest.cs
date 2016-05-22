using System;
using Newtonsoft.Json;

namespace Kraken.Model.Azure
{
    public class OptimizeWaitRequest : Model.OptimizeWaitRequest
    {
        public OptimizeWaitRequest(Uri imageUrl, DataStore dataStore) : base(imageUrl)
        {
            BlobStore = dataStore;
        }

        public OptimizeWaitRequest(Uri imageUrl, string account, string key, string container) : base(imageUrl)
        {
            BlobStore = new DataStore(account, key, container);
        }

        public OptimizeWaitRequest(Uri imageUrl, string account, string key, string container, string path)
            : base(imageUrl)
        {
            BlobStore = new DataStore(account, key, container, path);
        }

        [JsonProperty("azure_store")]
        public DataStore BlobStore { get; internal set; }
    }
}