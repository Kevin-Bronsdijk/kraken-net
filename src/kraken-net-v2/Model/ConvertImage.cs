using Newtonsoft.Json;

namespace Kraken.Model
{
    public class ConvertImage
    {
        public ConvertImage()
        {
            BackgroundColor = "#ffffff";
        }

        public ConvertImage(ImageFormat format)
        {
            Format = format;
            BackgroundColor = "#ffffff";
        }

        [JsonProperty("format")]
        public ImageFormat Format { get; set; }

        [JsonProperty("background")]
        public string BackgroundColor { get; set; }
    }
}