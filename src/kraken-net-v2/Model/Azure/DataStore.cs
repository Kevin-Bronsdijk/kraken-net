using System.Collections.Generic;
using Kraken.Logic;
using Newtonsoft.Json;

namespace Kraken.Model.Azure
{
    public class DataStore : IDataStore
    {
        public DataStore(string account, string key, string container)
        {
            Account = account;
            Key = key;
            Container = container;
        }

        public DataStore(string account, string key, string container, string path) :
            this(account, key, container)
        {
            Path = path;
        }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("container")]
        public string Container { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; } = "/";

        [JsonProperty("headers")]
        internal Dictionary<string, string> Headers { get; set; }

        [JsonProperty("metadata")]
        internal Dictionary<string, string> Metadata { get; set; }

        public void AddHeaders(string key, string value)
        {
            key.ThrowIfNullOrEmpty("key");
            value.ThrowIfNullOrEmpty("value");

            if (Headers == null)
            {
                Headers = new Dictionary<string, string>();
            }

            Headers.Add(key, value);
        }

        public void AddMetadata(string key, string value)
        {
            key.ThrowIfNullOrEmpty("key");
            value.ThrowIfNullOrEmpty("value");

            if (Metadata == null)
            {
                Metadata = new Dictionary<string, string>();
            }

            // Remove prefix, added by Kraken
            if (key.ToLower().StartsWith("x-ms-meta-"))
            {
                key = key.Replace("x-ms-meta-", string.Empty);
            }

            Metadata.Add(key, value);
        }
    }
}