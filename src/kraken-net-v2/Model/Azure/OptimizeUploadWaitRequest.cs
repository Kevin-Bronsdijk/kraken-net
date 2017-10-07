using Newtonsoft.Json;

namespace Kraken.Model.Azure
{
    public class OptimizeUploadWaitRequest : Model.OptimizeUploadWaitRequest
    {
        public OptimizeUploadWaitRequest(DataStore dataStore)
        {
            BlobStore = dataStore;
        }

        public OptimizeUploadWaitRequest(string account, string key, string container)
        {
            BlobStore = new DataStore(account, key, container);
        }

        public OptimizeUploadWaitRequest(string account, string key, string container, string path)
        {
            BlobStore = new DataStore(account, key, container, path);
        }

        [JsonProperty("azure_store")]
        public DataStore BlobStore { get; internal set; }
    }
}