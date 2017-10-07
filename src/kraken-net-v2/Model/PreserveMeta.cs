using System;

namespace Kraken.Model
{
    [Flags]
    public enum PreserveMeta
    {
        // All = 1,
        Date = 2,
        Copyright = 4,
        Geotag = 8,
        Orientation = 16,
        Profile = 32
    }
}