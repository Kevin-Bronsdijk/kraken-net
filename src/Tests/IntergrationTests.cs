using System;
using System.Drawing;
using System.IO;
using System.Net;
using Kraken.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using OptimizeRequest = Kraken.Model.OptimizeRequest;
using OptimizeUploadRequest = Kraken.Model.OptimizeUploadRequest;
using OptimizeUploadWaitRequest = Kraken.Model.OptimizeUploadWaitRequest;
using OptimizeWaitRequest = Kraken.Model.OptimizeWaitRequest;

namespace Tests
{
    [TestClass]
    //[Ignore]
    [DeploymentItem("Images")]
    public class IntergrationTests
    {
        // Not checking the results of the webhooks
        private readonly Uri _callbackUri = new Uri("http://requestb.in/15gm5dz1");

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void Client_Unauthorized_IsTrue()
        {
            Given.AClient.ThatHasAValidConnection().OptimizeWait(
                Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb())
                .Result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void Client_UnauthorizedSandbox_IsTrue()
        {
            Given.AClient.ThatHasAValidConnection(true).OptimizeWait(
                    Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb())
                .Result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void Client_GetUserStatus_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().UserStatus().Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Success.ShouldBeTrue();
            testitem.Body.PlanName.ShouldNotBeNullOrEmpty();
            testitem.Body.Active.ShouldNotBeNull();
            testitem.Body.QuotaTotal.ShouldBeGreaterThan(-0);
            testitem.Body.QuotaUsed.ShouldBeGreaterThan(-0);
            testitem.Body.QuotaRemaining.ShouldBeGreaterThan(-999999);
        }

        [TestMethod]
        public void Client_InvalidResource404_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AnExternalImageUrl.ThatReturns404ImageLocation()).Result;

            testitem.StatusCode.ShouldBe((HttpStatusCode)422);
            testitem.Success.ShouldBeFalse();
            testitem.Error.ShouldNotBeNullOrEmpty();
        }

        [TestMethod]
        public void Client_IgnoreInvalidResource404Sandbox_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect(true).OptimizeWait(
                Given.AnExternalImageUrl.ThatReturns404ImageLocation()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Success.ShouldBeTrue();
            testitem.Body.FileName.ShouldNotBeNullOrEmpty();
            testitem.Body.KrakedSize.ShouldBeGreaterThan(0);
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
            testitem.Body.OriginalSize.ShouldBeGreaterThan(0);
            testitem.Body.SavedBytes.ShouldBeGreaterThanOrEqualTo(0);
        }

        [TestMethod]
        public void Client_OptimizeWait_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public void Client_OptimizeCheckResultBody_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Success.ShouldBeTrue();
            testitem.Body.FileName.ShouldNotBeNullOrEmpty();
            testitem.Body.KrakedSize.ShouldBeGreaterThan(0);
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
            testitem.Body.OriginalSize.ShouldBeGreaterThan(0);
            testitem.Body.SavedBytes.ShouldBeGreaterThanOrEqualTo(0);
        }

        [TestMethod]
        public void Client_OptimizeCallbackCheckResultBody_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().Optimize(
                Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb(),
                Given.AnExternalImageUrl.ThatIsAValidCallBackUrl()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.ShouldNotBeNull();
            testitem.Body.Id.ShouldNotBeNullOrEmpty();
        }

        [TestMethod]
        public void Client_CustomRequestOptimizeCheckResultBody_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AOptimizeWaitRequest.ThatHasLossySetAsTrue()
                ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Success.ShouldBeTrue();
            testitem.Body.FileName.ShouldNotBeNullOrEmpty();
            testitem.Body.KrakedSize.ShouldBeGreaterThan(0);
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
            testitem.Body.OriginalSize.ShouldBeGreaterThan(0);
            testitem.Body.SavedBytes.ShouldBeGreaterThanOrEqualTo(0);
        }

        [TestMethod]
        public void Client_CustomRequestConvertImageFormat_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AOptimizeWaitRequest.ThatSetsTheImageFormatToGif()
            ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Success.ShouldBeTrue();
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
            testitem.Body.KrakedUrl.ToLower().ShouldEndWith(".gif");
        }

        [TestMethod]
        public void Client_CustomRequestConvertImageFormatEmptyConstructor_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AOptimizeWaitRequest.ThatConvertsTheImageToGifWithABackgroundColor()
            ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Success.ShouldBeTrue();
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
            testitem.Body.KrakedUrl.ToLower().ShouldEndWith(".gif");
        }

        [TestMethod]
        public void Client_CustomRequestChangeSize_IsTrue()
        {
            var result = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AOptimizeWaitRequest.ThatResizesTheImage()
            ).Result;

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);
            var testitem = Image.FromFile(localFile);

            testitem.Height.ShouldBe(100);
            testitem.Width.ShouldBe(100);
        }

        [TestMethod]
        public void Client_CustomRequestChangeSizeSquare_IsTrue()
        {
            var result = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AOptimizeWaitRequest.ThatResizesTheImageIntoASquare()
            ).Result;

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);
            var testitem = Image.FromFile(localFile);

            testitem.Height.ShouldBe(120);
            testitem.Width.ShouldBe(120);
        }

        [TestMethod]
        public void Client_OptimizeRequestCallback_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().Optimize(
                Given.AOptimizeRequest.ThatHasAnExistingImageAndValidCallBackUri()
            ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.ShouldNotBeNull();
            testitem.Body.Id.ShouldNotBeNullOrEmpty();
        }

        [TestMethod]
        public void Client_OptimizeRequestCallbackSandbox_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect(true).Optimize(
                 Given.AOptimizeRequest.ThatHasAnExistingImageAndValidCallBackUri()
             ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.ShouldNotBeNull();
            // No id in sandbox mode
            testitem.Body.Id.ShouldBeNullOrEmpty();
        }

        [TestMethod]
        public void Client_UploadImageWaitResult_IsTrue()
        {
            var testImageName = TestData.TestImageName;
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = client.OptimizeWait(
                image,
                testImageName,
                new OptimizeUploadWaitRequest()
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);

            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.FileName == testImageName);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.EndsWith(testImageName));
            Assert.IsTrue(result.Body.KrakedSize >= 0);
            Assert.IsTrue(result.Body.OriginalSize >= 0);
            Assert.IsTrue(result.Body.SavedBytes >= 0);
        }

        [TestMethod]
        public void Client_UploadImageCallback_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = client.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest(_callbackUri)
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_CustomRequestUploadWait_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = client.OptimizeWait(
                image,
                TestData.TestImageName,
                new OptimizeUploadWaitRequest
                {
                    ResizeImage = new ResizeImage { Height = 100, Width = 100 },
                    WebP = true
                }
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
        }

        [TestMethod]
        public void Client_CustomRequestUploadCallback_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = client.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest(_callbackUri)
                {
                    ResizeImage = new ResizeImage { Height = 100, Width = 100 },
                    WebP = true
                }
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_UploadFromFilePathImageWait_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                TestData.LocalTestImage,
                new OptimizeUploadWaitRequest()
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
        }

        [TestMethod]
        public void Client_UploadFromFilePathImageCallback_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Optimize(
                TestData.LocalTestImage,
                new OptimizeUploadRequest(_callbackUri)
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_SamplingScheme_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var optimizeUploadWaitRequest = new OptimizeUploadWaitRequest()
            {
                Lossy = true,
                WebP = true,
                SamplingScheme = SamplingScheme.S444
            };

            Assert.IsTrue(optimizeUploadWaitRequest.SamplingScheme == SamplingScheme.S444);
            
            var response = client.OptimizeWait(
                TestData.LocalTestImage,
                optimizeUploadWaitRequest
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
        }

        [TestMethod]
        public void Client_AutoOrient_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                TestData.LocalTestImage,
                new OptimizeUploadWaitRequest()
                {
                    AutoOrient = true
                }
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
        }

        [TestMethod]
        public void Client_SimpleRequetsNoBody_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                TestData.LocalTestImage
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
        }

        [TestMethod]
        public void Client_SimpleCallbackRequetsNoBody_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Optimize(
                TestData.LocalTestImage, _callbackUri
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_ImageSetUploadCallBack_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeSetUploadRequest(_callbackUri)
            {
                Lossy = true,
            };
            request.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10 });
            request.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15 });
            request.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20 });

            var response = client.Optimize(TestData.LocalTestImage,
                request
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }


        [TestMethod]
        public void Client_ImageSetUrlCallBack_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeSetRequest(new Uri(TestData.ImageOne), _callbackUri)
            {
                Lossy = true,
            };
            request.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10 });
            request.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15 });
            request.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20 });

            var response = client.Optimize(
                request
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_ImageSetUrlWait_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeSetWaitRequest(new Uri(TestData.ImageOne))
            {
                Lossy = true,
            };
            request.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10 });
            request.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15 });
            request.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20 });

            var response = client.OptimizeWait(
                request
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);

            Assert.IsTrue(result.Body.Results.Count == 3);

            foreach (var item in result.Body.Results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.FileName));
                Assert.IsTrue(item.KrakedSize > 0);
                Assert.IsTrue(!string.IsNullOrEmpty(item.KrakedUrl));
                Assert.IsTrue(item.OriginalSize > 0);
                Assert.IsTrue(item.SavedBytes > 0);
            }
        }

        [TestMethod]
        public void Client_ImageSetUploadWait_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeSetUploadWaitRequest()
            {
                Lossy = true,
            };
            request.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10 });
            request.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15 });
            request.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20 });

            var response = client.OptimizeWait(TestData.LocalTestImage,
                request
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);

            Assert.IsTrue(result.Body.Results.Count == 3);

            foreach (var item in result.Body.Results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.FileName));
                Assert.IsTrue(item.KrakedSize > 0);
                Assert.IsTrue(!string.IsNullOrEmpty(item.KrakedUrl));
                Assert.IsTrue(item.OriginalSize > 0);
                Assert.IsTrue(item.SavedBytes > 0);
            }
        }

        [TestMethod]
        public void Client_ImageSetUploadWaitOverridingParameters_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeSetUploadWaitRequest()
            {
                Lossy = true,
            };
            request.AddSet(new ResizeImageSet {
                Name = "test1", Height = 10, Width = 10, Lossy = false
            });
            request.AddSet(new ResizeImageSet {
                Name = "test2", Height = 15, Width = 15, SamplingScheme = SamplingScheme.S444
            });

            var response = client.OptimizeWait(TestData.LocalTestImage,
                request
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);

            Assert.IsTrue(result.Body.Results.Count == 2);

            foreach (var item in result.Body.Results)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(item.FileName));
                Assert.IsTrue(item.KrakedSize > 0);
                Assert.IsTrue(!string.IsNullOrEmpty(item.KrakedUrl));
                Assert.IsTrue(item.OriginalSize > 0);
                Assert.IsTrue(item.SavedBytes > 0);
            }
        }

        [TestMethod]
        public void Client_OptimizeCheckCustomQuality_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                Lossy = true,
                Quality = 90
            };

            var response = client.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);            
            
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes >= 0);
        }
    }
}