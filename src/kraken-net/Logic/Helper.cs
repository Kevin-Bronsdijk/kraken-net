using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Model;

namespace Kraken.Logic
{
    internal static class Helper
    {
        public static void ThrowIfNullOrEmpty(this string value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            if (value == string.Empty)
            {
                throw new ArgumentException("Argument must not be the empty string.", name);
            }
        }

        public static string GetSamplingScheme(SamplingScheme samplingScheme)
        {
            var samplingSchemes = new Dictionary<string, string>
            {
                {"Default", "4:2:0"},
                {"S422", "4:2:2"},
                {"S444", "4:2:4"}
            };

            string chroma;
            samplingSchemes.TryGetValue(samplingScheme.ToString(), out chroma);

            return chroma;
        }
    }
}