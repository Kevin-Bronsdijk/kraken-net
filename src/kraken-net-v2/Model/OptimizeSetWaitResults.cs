using System.Collections.Generic;

namespace Kraken.Model
{
    public class OptimizeSetWaitResults
    {
        public List<OptimizeSetWaitResult> Results { get; set; } = new List<OptimizeSetWaitResult>();
        public bool Success { get; set; }
    }
}
