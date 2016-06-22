using System;
using System.IO;
using System.Net;
using Kraken.Model;
using Kraken.Model.S3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimizeRequest = Kraken.Model.S3.OptimizeRequest;
using OptimizeUploadRequest = Kraken.Model.S3.OptimizeUploadRequest;
using OptimizeUploadWaitRequest = Kraken.Model.S3.OptimizeUploadWaitRequest;
using OptimizeWaitRequest = Kraken.Model.S3.OptimizeWaitRequest;

namespace Tests
{
    [TestClass]
    [Ignore]
    [DeploymentItem("Images")]
    public class IntergrationTestsAmazon
    {
        // Not checking the results of the webhooks
        private readonly Uri _callbackUri = new Uri("http://requestb.in/15gm5dz1");

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void Client_OptimizeWaitAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                new Uri(TestData.ImageOne),
                 new DataStore(
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty
                    )
                ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void Client_OptimizeCallbackAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Optimize(
                  new OptimizeRequest(
                  new Uri(TestData.ImageOne),
                  _callbackUri,
                   new DataStore(
                      Settings.AmazonKey,
                      Settings.AmazonSecret,
                      Settings.AmazonBucket,
                      string.Empty
                      )
                  ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_OptimizeCallbackAmazonWithParams_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Optimize(
                  new OptimizeRequest(
                  new Uri(TestData.ImageOne),
                  _callbackUri,
                  Settings.AmazonKey,
                  Settings.AmazonSecret,
                  Settings.AmazonBucket,
                  string.Empty
                  ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_UploadOptimizeWaitAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = client.OptimizeWait(
                image,
                TestData.TestImageName,
                new OptimizeUploadWaitRequest(
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void Client_UploadOptimizeWaitAmazonDataStore_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = client.OptimizeWait(
                image,
                TestData.TestImageName,
                new OptimizeUploadWaitRequest(new DataStore(
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty)
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void Client_UploadOptimizeCallbackAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = client.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest(
                    _callbackUri,
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_UploadOptimizeCallbackAmazonDataStore_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(TestData.LocalTestImage);

            var response = client.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest(
                    _callbackUri, new DataStore(
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty)
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [TestMethod]
        public void Client_OptimizeWaitAmazonUsingIOptimizeWaitRequestDataStore_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    new DataStore(
                        Settings.AmazonKey,
                        Settings.AmazonSecret,
                        Settings.AmazonBucket,
                        string.Empty
                        )
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void Client_OptimizeWaitAmazonUsingIOptimizeWaitRequest_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    Settings.AmazonKey,
                    Settings.AmazonSecret,
                    Settings.AmazonBucket,
                    string.Empty
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void Client_OptimizeWaitAmazonWithAcl_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                     new DataStore(
                        Settings.AmazonKey,
                        Settings.AmazonSecret,
                        Settings.AmazonBucket,
                        string.Empty)
                     {
                         Acl = "public_read"
                     }
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void Client_OptimizeWaitAmazonWithPath_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                     new DataStore(
                        Settings.AmazonKey,
                        Settings.AmazonSecret,
                        Settings.AmazonBucket,
                        string.Empty)
                     {
                         Path = "test/"
                     }
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

        [TestMethod]
        public void Client_OptimizeWaitAmazonAddHeadersAndMeta_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var dataStore = new DataStore(
                Settings.AmazonKey,
                Settings.AmazonSecret,
                Settings.AmazonBucket,
                string.Empty
                );

            dataStore.AddMetadata("x-amz-meta-test1", "value11"); // Prefix will be removeda and added by kraken later
            dataStore.AddMetadata("test2", "value22");
            dataStore.AddHeaders("Cache-Control", "max-age=2222");

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    dataStore
                    )
                {
                    ResizeImage = new ResizeImage { Height = 100, Width = 100 },
                    WebP = true
                }
            );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("s3.amazonaws.com"));
        }

    }
}