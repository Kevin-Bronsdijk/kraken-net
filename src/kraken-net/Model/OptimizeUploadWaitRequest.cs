using Newtonsoft.Json;

namespace Kraken.Model
{
    public class OptimizeUploadWaitRequest : OptimizeRequestBase, IOptimizeUploadWaitRequest
    {
        [JsonProperty("wait")]
        internal bool Wait { get; } = true;
    }
}