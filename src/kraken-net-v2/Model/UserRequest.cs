using Newtonsoft.Json;

namespace Kraken.Model
{
    public class UserRequest : IRequest
    {
        public UserRequest()
        {
            Authentication = new Authentication();
        }

        [JsonProperty("auth")]
        public Authentication Authentication { get; set; }

        [JsonIgnore]
        public bool Dev { get; set; }
    }
}