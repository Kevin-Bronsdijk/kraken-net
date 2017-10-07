using System;
using Newtonsoft.Json;

namespace Kraken.Model.Azure
{
    public class OptimizeRequest : Model.OptimizeRequest
    {
        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, DataStore dataStore) : base(imageUrl, callbackUrl)
        {
            BlobStore = dataStore;
        }

        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, string account, string key, string container)
            : base(imageUrl, callbackUrl)
        {
            BlobStore = new DataStore(account, key, container);
        }

        public OptimizeRequest(Uri imageUrl, Uri callbackUrl, string account, string key, string container, string path)
            : base(imageUrl, callbackUrl)
        {
            BlobStore = new DataStore(account, key, container, path);
        }

        [JsonProperty("azure_store")]
        public DataStore BlobStore { get; internal set; }
    }
}