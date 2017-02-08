using System;
using System.Drawing;
using System.IO;
using System.Net;
using Kraken;
using Kraken.Http;
using Kraken.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimizeRequest = Kraken.Model.OptimizeRequest;
using OptimizeUploadRequest = Kraken.Model.OptimizeUploadRequest;
using OptimizeUploadWaitRequest = Kraken.Model.OptimizeUploadWaitRequest;
using OptimizeWaitRequest = Kraken.Model.OptimizeWaitRequest;

namespace Tests
{
    [TestClass]
    [Ignore]
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
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            var response = client.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void Client_UnauthorizedSandbox_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            var response = client.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void Client_GetUserStatus_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.UserStatus();

            var result = response.Result;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);

            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.PlanName));
            Assert.IsTrue(result.Body.Active || result.Body.Active == false);
            Assert.IsTrue(result.Body.QuotaTotal > -0);
            Assert.IsTrue(result.Body.QuotaUsed > -0);
            Assert.IsTrue(result.Body.QuotaRemaining > -999999);
            Assert.IsTrue(result.Body.Success);
        }

        [TestMethod]
        public void Client_InvalidResource404_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new Uri(TestData.Image404));

            var result = response.Result;

            Assert.IsTrue((int)result.StatusCode == 422);
            Assert.IsTrue(result.Success == false);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Error));
        }

        [TestMethod]
        public void Client_IgnoreInvalidResource404Sandbox_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient(true);

            var response = client.OptimizeWait(
                new Uri(TestData.Image404));

            var result = response.Result;

            // Should still work, file ignored because getting random results
            // when in developer mode
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);

            Assert.IsTrue(result.Body.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes > 0);
        }

        [TestMethod]
        public void Client_OptimizeWait_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void Client_OptimizeCheckResultBody_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new Uri(TestData.ImageOne));

            var result = response.Result;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);

            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes >= 0);
        }

        [TestMethod]
        public void Client_OptimizeCallbackCheckResultBody_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Optimize(
                new Uri(TestData.ImageOne), _callbackUri);

            var result = response.Result;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_CustomRequestOptimizeCheckResultBody_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                Lossy = true
            };

            var response = client.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);

            // Can only check if we have data, not checking if lossy has been applied
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.FileName));
            Assert.IsTrue(result.Body.KrakedSize > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.OriginalSize > 0);
            Assert.IsTrue(result.Body.SavedBytes >= 0);
        }

        [TestMethod]
        public void Client_CustomRequestConvertImageFormat_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                ConvertImage = new ConvertImage(ImageFormat.Gif)
            };

            var response = client.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.ToLower().EndsWith(".gif"));
        }

        [TestMethod]
        public void Client_CustomRequestConvertImageFormatEmptyConstructor_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var convertImage = new ConvertImage()
            {
                BackgroundColor = "#ffffff",
                Format = ImageFormat.Gif
            };

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                ConvertImage = convertImage
            };

            var response = client.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.ToLower().EndsWith(".gif"));
        }

        [TestMethod]
        public void Client_CustomRequestChangeSize_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                ResizeImage = new ResizeImage
                {
                    Height = 100,
                    Width = 100,
                    BackgroundColor = "#ffffff",
                    Strategy = Strategy.Exact,
                    CropMode = "c"
                }
            };

            var response = client.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);

            var img = Image.FromFile(localFile);
            Assert.IsTrue(img.Height == 100);
            Assert.IsTrue(img.Width == 100);
        }

        [TestMethod]
        public void Client_CustomRequestChangeSizeSquare_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                ResizeImage = new ResizeImage
                {
                    Size = 120,
                    Strategy = Strategy.Square,
                }
            };

            var response = client.OptimizeWait(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));

            var localFile = HelperFunctions.DownloadImage(result.Body.KrakedUrl);

            var img = Image.FromFile(localFile);
            Assert.IsTrue(img.Height == 120);
            Assert.IsTrue(img.Width == 120);
        }

        [TestMethod]
        public void Client_OptimizeRequestCallback_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var request = new OptimizeRequest(new Uri(TestData.ImageOne), _callbackUri);

            var response = client.Optimize(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_OptimizeRequestCallbackSandbox_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient(true);

            var request = new OptimizeRequest(new Uri(TestData.ImageOne), _callbackUri);

            var response = client.Optimize(request);
            var result = response.Result;

            Assert.IsTrue(result.Body != null);
            // No id in sandbox mode
            Assert.IsTrue(result.Body.Id == null);
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

        //[TestMethod]
        //public void Client_WaitNoRequestBody_IsTrue()
        //{
        //    var client = HelperFunctions.CreateWorkingClient();

        //    try
        //    {
        //        var response = client.OptimizeWait(
        //            TestData.LocalTestImage, null
        //            );

        //        Assert.IsTrue(response == null);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        Assert.IsTrue(ex.Message == "Value cannot be null.\r\nParameter name: optimizeWaitRequest");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.IsTrue(false, ex.Message);
        //    }
        //}

        //[TestMethod]
        //public void Client_CallbackNoRequestBody_IsTrue()
        //{
        //    var client = HelperFunctions.CreateWorkingClient();

        //    try
        //    {
        //        var response = client.Optimize(null);

        //        Assert.IsTrue(response == null);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        Assert.IsTrue(ex.Message == "Value cannot be null.\r\nParameter name: optimizeRequest");
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.IsTrue(false, ex.Message);
        //    }
        //}

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