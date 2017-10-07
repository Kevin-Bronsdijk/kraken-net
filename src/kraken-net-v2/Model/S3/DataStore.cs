using System.Collections.Generic;
using Kraken.Logic;
using Newtonsoft.Json;

namespace Kraken.Model.S3
{
    public class DataStore : IDataStore
    {
        public DataStore(string key, string secret, string bucket, string region)
        {
            Key = key;
            Secret = secret;
            Bucket = bucket;
            Region = region;
        }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("bucket")]
        public string Bucket { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("acl")]
        public string Acl { get; set; } = "public_read";

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
            if (key.ToLower().StartsWith("x-amz-meta-"))
            {
                key = key.Replace("x-amz-meta-", string.Empty);
            }

            Metadata.Add(key, value);
        }
    }
}