using Newtonsoft.Json;

namespace Kraken.Model
{
    public class OptimizeSetUploadWaitRequest : OptimizeSetRequestBase, IOptimizeSetUploadWaitRequest
    {
        [JsonProperty("wait")]
        internal bool Wait { get; set; } = true;
    }
}