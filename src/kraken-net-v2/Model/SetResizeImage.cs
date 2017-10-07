using Kraken.Logic;
using Newtonsoft.Json;

namespace Kraken.Model
{
    public class ResizeImageSet : ResizeImage
    {
        private SamplingScheme _samplingScheme;

        [JsonProperty("id")]
        public string Name { get; set; }

        [JsonProperty("storage_path")]
        public string StoragePath { get; set; }

        [JsonProperty("lossy")]
        public bool Lossy { get; set; } = false;

        [JsonIgnore]
        public SamplingScheme SamplingScheme
        {
            get { return _samplingScheme; }
            set
            {
                // I think this clearly shows that Its better to use separate models 
                // (client & internal) and convert data when converting the model to a 
                // API model. In this way, you don’t have to create internal properties or ignore properties.

                _samplingScheme = value;
                SamplingSchemeInternal = Helper.GetSamplingScheme(_samplingScheme);
            }
        }

        [JsonProperty("sampling_scheme")]
        internal string SamplingSchemeInternal { get; set; }
    }
}
