using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Kraken.Model
{
    public class OptimizeSetRequestBase : OptimizeRequestBase
    {
        public void AddSet(ResizeImageSet resizeImage)
        {
            if (resizeImage == null) throw new ArgumentException();

            if (ResizeImage != null)
            { 
                // Only 10 items allowed per request
                if (ResizeImage.Count == 10)
                {
                    throw new Exception("Cannot exceed the quota of 10 instructions per request");
                }

                // Name must be unique
                if (ResizeImage.Any(s => s.Name == resizeImage.Name))
                {
                    throw new Exception("Item already exists in collection");
                }
            }

            if (ResizeImage == null) { ResizeImage = new List<ResizeImageSet>(); }
            this.ResizeImage.Add(resizeImage);
        }

        [JsonProperty("resize")]
        public new List<ResizeImageSet> ResizeImage { get; private set; }
    }
}
