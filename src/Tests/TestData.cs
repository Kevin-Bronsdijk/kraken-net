using System;

namespace Tests
{
    public static class TestData
    {
        public const string ImageOne = "https://kraken.io/assets/images/kraken-logo-4.png";
        public const string Image404 = "https://kraken.io/im-out-for-lunch.png";
        // Todo: Needs to be included within the test project, but dont want any copyright issues... 
        public const string ImageGeoTag =
            "http://www.geoimgr.com/s/photos/micmic_6fcdeae8cd4ae3d2a7dcf9aa24abebee_l.jpg";

        public const string LocalTestImage = "Kraken.png";
        public static string TestImageName => "test" + Guid.NewGuid() + ".png";
    }
}