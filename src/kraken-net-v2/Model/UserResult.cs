using Newtonsoft.Json;

namespace Kraken.Model
{
    public class UserResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("plan_name")]
        public string PlanName { get; set; }

        [JsonProperty("quota_total")]
        public double QuotaTotal { get; set; }

        [JsonProperty("quota_used")]
        public double QuotaUsed { get; set; }

        [JsonProperty("quota_remaining")]
        public double QuotaRemaining { get; set; }
    }
}