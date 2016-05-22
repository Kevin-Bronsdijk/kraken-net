using System;
using System.Net;
using ExifLib;
using Kraken.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimizeWaitRequest = Kraken.Model.OptimizeWaitRequest;

namespace Tests
{
    [TestClass]
    [DeploymentItem("Images")]
    public class ExifTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void Client_CustomRequestRemoveGeoData_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageGeoTag));

            var response = client.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);

            try
            {
                // Will fail is there isn't any Exif data
                using (var reader = new ExifReader(localFile))
                {
                }

                Assert.IsTrue(false, "No Exception");
            }
            catch (Exception)
            {
                Assert.IsTrue(true, "No Exif data");
            }
        }

        [TestMethod]
        public void Client_CustomRequestKeepGeoData_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageGeoTag))
            {
                PreserveMeta = new[] { PreserveMeta.Geotag }
            };

            var response = client.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);

            try
            {
                using (var reader = new ExifReader(localFile))
                {
                    double[] gpsLongArray;
                    double[] gpsLatArray;
                    double gpsLongDouble = 0;
                    double gpsLatDouble = 0;

                    if (reader.GetTagValue(ExifTags.GPSLongitude, out gpsLongArray)
                        && reader.GetTagValue(ExifTags.GPSLatitude, out gpsLatArray))
                    {
                        gpsLongDouble = gpsLongArray[0] + gpsLongArray[1] / 60 + gpsLongArray[2] / 3600;
                        gpsLatDouble = gpsLatArray[0] + gpsLatArray[1] / 60 + gpsLatArray[2] / 3600;
                    }

                    Assert.IsTrue(gpsLongDouble == 120.7602005);
                    Assert.IsTrue(gpsLatDouble == 21.958606694444445);
                }
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "No Exif data");
            }
        }
    }
}
