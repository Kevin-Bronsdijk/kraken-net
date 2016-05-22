using System;
using Newtonsoft.Json;

namespace Kraken.Model.Azure
{
    public class OptimizeUploadRequest : Model.OptimizeUploadRequest
    {
        public OptimizeUploadRequest(Uri callbackUrl, DataStore dataStore) : base(callbackUrl)
        {
            BlobStore = dataStore;
        }

        public OptimizeUploadRequest(Uri callbackUrl, string account, string key, string container) : base(callbackUrl)
        {
            BlobStore = new DataStore(account, key, container);
        }

        public OptimizeUploadRequest(Uri callbackUrl, string account, string key, string container, string path)
            : base(callbackUrl)
        {
            BlobStore = new DataStore(account, key, container, path);
        }

        [JsonProperty("azure_store")]
        public DataStore BlobStore { get; internal set; }
    }
}