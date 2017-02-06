using Kraken.Logic;
using Newtonsoft.Json;

namespace Kraken.Model
{
    public abstract class OptimizeRequestBase : IRequest
    {
        private SamplingScheme _samplingScheme;

        [JsonProperty("lossy")]
        public bool Lossy { get; set; } = false;

        [JsonProperty("webp")]
        public bool WebP { get; set; } = false;

        [JsonProperty("auto_orient")]
        public bool AutoOrient { get; set; } = false;

        [JsonProperty("convert")]
        public ConvertImage ConvertImage { get; set; }

        [JsonProperty("resize")]
        public ResizeImage ResizeImage { get; set; }

        [JsonProperty("preserve_meta")]
        public PreserveMeta[] PreserveMeta { get; set; }

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

        [JsonProperty("auth")]
        public Authentication Authentication { get; } = new Authentication();

        [JsonProperty("dev")]
        public bool Dev { get; set; }

        [JsonProperty("quality")]
        public int Quality { get; set; }
    }
}