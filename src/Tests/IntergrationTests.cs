using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    [TestFixture]
    [Ignore("Ignore for CI")]
    public class IntergrationTests
    {
        private static string GetPathResources(string nameResourse)
        {
            var path = Path.GetDirectoryName(path: new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            return $"{path}\\images\\{nameResourse}";
        }
        
        [Test]
        public void Initialize()
        {
        }

        [Test]
        public void Client_Unauthorized_IsTrue()
        {
            Given.AClient.ThatHasAValidConnection().OptimizeWait(
                Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb())
                .Result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Test]
        public void Client_UnauthorizedSandbox_IsTrue()
        {
            Given.AClient.ThatHasAValidConnection(true).OptimizeWait(
                    Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb())
                .Result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Test]
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

        [Test]
        public void Client_InvalidResource404_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AnExternalImageUrl.ThatReturns404ImageLocation()).Result;

            testitem.StatusCode.ShouldBe((HttpStatusCode)422);
            testitem.Success.ShouldBeFalse();
            testitem.Error.ShouldNotBeNullOrEmpty();
        }

        [Test]
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

        [Test]
        public void Client_OptimizeWait_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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


        [Test]
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

        [Test]
        public void Client_CustomRequestChangeSizeStrategyNone_IsTrue()
        {
            var result = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AOptimizeWaitRequest.ThatResizesTheImageWithStrategyNone()
            ).Result;

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);
            var testitem = Image.FromFile(localFile);

            testitem.Height.ShouldBe(37);
            testitem.Width.ShouldBe(150);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Client_UploadImageWaitResult_IsTrue()
        {
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));
            var testImageName = TestData.TestImageName;
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                image,
                testImageName,
                Given.AOptimizeUploadWaitRequest.ThatInitialOptimizeUploadWaitRequest()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Success.ShouldBeTrue();
            testitem.Body.FileName.ShouldNotBeNullOrEmpty();
            testitem.Body.KrakedSize.ShouldBeGreaterThan(0);
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
            testitem.Body.OriginalSize.ShouldBeGreaterThan(0);
            testitem.Body.SavedBytes.ShouldBeGreaterThanOrEqualTo(0);
        }

        [Test]
        public void Client_UploadImageCallback_IsTrue()
        {
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));
            var testitem = Given.AClient.ThatCanConnect().Optimize(
                image,
                TestData.TestImageName,
                Given.AOptimizeUploadRequest.ThatHasAValidCallbackUrl()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Id.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_CustomRequestUploadWait_IsTrue()
        {
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                image,
                GetPathResources(TestData.LocalTestImage),
                Given.AOptimizeUploadWaitRequest.ThatHasResizeOptions()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_CustomRequestUploadCallback_IsTrue()
        {
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));
            var testitem = Given.AClient.ThatCanConnect().Optimize(
                image,
                GetPathResources(TestData.LocalTestImage),
                Given.AOptimizeUploadRequest.ThatHasAValidCallbackUrl()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Id.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_UploadFromFilePathImageWait_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                GetPathResources(TestData.LocalTestImage),
                Given.AOptimizeUploadWaitRequest.ThatInitialOptimizeUploadWaitRequest()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_UploadFromFilePathImageCallback_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().Optimize(
                GetPathResources(TestData.LocalTestImage),
                Given.AOptimizeUploadRequest.ThatHasAValidCallbackUrl()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Id.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_SamplingScheme_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                GetPathResources(TestData.LocalTestImage),
                Given.AOptimizeUploadWaitRequest.ThatHasLossyWebPAndSamplingScheme()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_AutoOrient_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                GetPathResources(TestData.LocalTestImage),
                Given.AOptimizeUploadWaitRequest.ThatHasAutoOrientOn()).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_SimpleRequetsNoBody_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                GetPathResources(TestData.LocalTestImage)).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_SimpleCallbackRequetsNoBody_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().Optimize(
                GetPathResources(TestData.LocalTestImage),
                Given.ACallBackUrl.ThatIsAValidCallBackUrl()
                ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Id.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_ImageSetUploadCallBack_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().Optimize(
                GetPathResources(TestData.LocalTestImage),
                Given.AOptimizeSetUploadRequest.ThatHasASetOf3()
            ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Id.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_ImageSetUrlCallBack_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().Optimize(
                Given.AOptimizeSetRequest.ThatHasASetOf3()
            ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Id.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void Client_ImageSetUrlWait_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AOptimizeSetWaitRequest.ThatHasASetOf3()
            ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Results.Count.ShouldBe(3);

            foreach (var item in testitem.Body.Results)
            {
                item.FileName.ShouldNotBeNullOrEmpty();
                item.KrakedUrl.ShouldNotBeNullOrEmpty();
            }
        }

        [Test]
        public void Client_ImageSetUploadWait_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                GetPathResources(TestData.LocalTestImage),
                Given.AOptimizeSetUploadWaitRequest.ThatHasASetOf3()
            ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Results.Count.ShouldBe(3);

            foreach (var item in testitem.Body.Results)
            {
                item.FileName.ShouldNotBeNullOrEmpty();
                item.KrakedUrl.ShouldNotBeNullOrEmpty();
            }
        }

        [Test]
        public void Client_ImageSetUploadWaitOverridingParameters_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                GetPathResources(TestData.LocalTestImage),
                Given.AOptimizeSetUploadWaitRequest.ThatHasASetOf2WithCustomSettings()
            ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Results.Count.ShouldBe(2);

            foreach (var item in testitem.Body.Results)
            {
                item.FileName.ShouldNotBeNullOrEmpty();
                item.KrakedUrl.ShouldNotBeNullOrEmpty();
            }
        }

        [Test]
        public void Client_OptimizeCheckCustomQuality_IsTrue()
        {
            var testitem = Given.AClient.ThatCanConnect().OptimizeWait(
                Given.AOptimizeWaitRequest.ThatSetsCustomQuality()
            ).Result;

            testitem.Success.ShouldBeTrue();
            testitem.StatusCode.ShouldBe(HttpStatusCode.OK);
            testitem.Body.Success.ShouldBeTrue();
            testitem.Body.KrakedUrl.ShouldNotBeNullOrEmpty();
            testitem.Body.KrakedSize.ShouldBeGreaterThan(0);
            testitem.Body.OriginalSize.ShouldBeGreaterThan(0);
            testitem.Body.SavedBytes.ShouldBeGreaterThanOrEqualTo(0);
        }
    }
}