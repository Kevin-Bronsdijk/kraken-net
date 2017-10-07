//using System;
//using System.Net;
//using ExifLib;
//using Kraken.Model;
//using Shouldly;
//using NUnit.Framework;

//namespace Tests
//{
//    [TestFixture]
//    [Ignore("Ignore for CI")]
//    public class ExifTests
//    {
//        [Test]
//        public void Client_CustomRequestRemoveGeoData_IsTrue()
//        {
//            var response = Given.AClient.ThatCanConnect().OptimizeWait(
//                Given.AOptimizeWaitRequest.ThatHasAUriToAnImageWithGeoTags()).Result;

//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//            response.Body.ShouldNotBeNull();
//            response.Body.KrakedUrl.ShouldNotBeNullOrEmpty();

//            var localFile = HelperFunctions.DownloadImage(response.Body.KrakedUrl);

//            // Will fail is there isn't any Exif data
//            Should.Throw<Exception>(() => new ExifReader(localFile));
//        }

//        [Test]
//        public void Client_CustomRequestKeepGeoData_IsTrue()
//        {
//            var response = Given.AClient.ThatCanConnect().OptimizeWait(
//                Given.AOptimizeWaitRequest.ThatHasAUriToAnImageWithGeoTags(new[] { PreserveMeta.Geotag })).Result;

//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//            response.Body.ShouldNotBeNull();
//            response.Body.KrakedUrl.ShouldNotBeNullOrEmpty();

//            var localFile = HelperFunctions.DownloadImage(response.Body.KrakedUrl);

//            using (var reader = new ExifReader(localFile))
//            {
//                double[] gpsLongArray;
//                double[] gpsLatArray;
//                double gpsLongDouble = 0;
//                double gpsLatDouble = 0;

//                if (reader.GetTagValue(ExifTags.GPSLongitude, out gpsLongArray)
//                    && reader.GetTagValue(ExifTags.GPSLatitude, out gpsLatArray))
//                {
//                    gpsLongDouble = gpsLongArray[0] + gpsLongArray[1] / 60 + gpsLongArray[2] / 3600;
//                    gpsLatDouble = gpsLatArray[0] + gpsLatArray[1] / 60 + gpsLatArray[2] / 3600;
//                }

//                gpsLongDouble.ShouldBe(120.7602005);
//                gpsLatDouble.ShouldBe(21.958606694444445);
//            }
//        }
//    }
//}
