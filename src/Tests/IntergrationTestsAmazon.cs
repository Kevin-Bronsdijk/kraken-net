using System;
using System.IO;
using System.Net;
using System.Reflection;
using Kraken.Model;
using Kraken.Model.S3;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using OptimizeRequest = Kraken.Model.S3.OptimizeRequest;
using OptimizeUploadRequest = Kraken.Model.S3.OptimizeUploadRequest;
using OptimizeUploadWaitRequest = Kraken.Model.S3.OptimizeUploadWaitRequest;
using OptimizeWaitRequest = Kraken.Model.S3.OptimizeWaitRequest;
using OptimizeSetRequest = Kraken.Model.S3.OptimizeSetRequest;
using OptimizeSetUploadRequest = Kraken.Model.S3.OptimizeSetUploadRequest;
using OptimizeSetUploadWaitRequest = Kraken.Model.S3.OptimizeSetUploadWaitRequest;
using OptimizeSetWaitRequest = Kraken.Model.S3.OptimizeSetWaitRequest;

namespace Tests
{
    [TestFixture]
    [Ignore("Ignore for CI")]
    public class IntergrationTestsAmazon
    {
        private static string GetPathResources(string nameResourse)
        {
            var path = Path.GetDirectoryName(path: new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            return $"{path}\\images\\{nameResourse}";
        }
        
        // Not checking the results of the webhooks
        private readonly Uri _callbackUri = new Uri("http://requestb.in/15gm5dz1");

        [Test]
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
            Assert.IsTrue(result.Body.KrakedUrl.Contains(".amazonaws.com"));
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Client_UploadOptimizeWaitAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

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
            Assert.IsTrue(result.Body.KrakedUrl.Contains(".amazonaws.com"));
        }

        [Test]
        public void Client_UploadOptimizeWaitAmazonDataStore_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

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
            Assert.IsTrue(result.Body.KrakedUrl.Contains(".amazonaws.com"));
        }

        [Test]
        public void Client_UploadOptimizeCallbackAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

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

        [Test]
        public void Client_UploadOptimizeCallbackAmazonDataStore_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

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

        [Test]
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
            Assert.IsTrue(result.Body.KrakedUrl.Contains(".amazonaws.com"));
        }

        [Test]
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
            Assert.IsTrue(result.Body.KrakedUrl.Contains(".amazonaws.com"));
        }

        [Test]
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
            Assert.IsTrue(result.Body.KrakedUrl.Contains(".amazonaws.com"));
        }

        [Test]
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
            Assert.IsTrue(result.Body.KrakedUrl.Contains(".amazonaws.com"));
        }

        [Test]
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
            Assert.IsTrue(result.Body.KrakedUrl.Contains(".amazonaws.com"));
        }

        [Test]
        public void Client_ImageSetUploadCallBackAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var dataStore = new DataStore(
                  Settings.AmazonKey,
                  Settings.AmazonSecret,
                  Settings.AmazonBucket,
                  string.Empty
                  );

            var request = new OptimizeSetUploadRequest(_callbackUri, dataStore)
            {
                Lossy = true,
            };
            request.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10, StoragePath = "test1/test1.png" });
            request.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15, StoragePath = "test2/test2.png" });
            request.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20, StoragePath = "test3/test3.png" });

            var response = client.Optimize(GetPathResources(TestData.LocalTestImage),
                request
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [Test]
        public void Client_ImageSetUrlCallBackAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var dataStore = new DataStore(
                  Settings.AmazonKey,
                  Settings.AmazonSecret,
                  Settings.AmazonBucket,
                  string.Empty
                  );

            var request = new OptimizeSetRequest(new Uri(TestData.ImageOne), _callbackUri, dataStore)
            {
                Lossy = true,
            };
            request.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10, StoragePath = "test1/test1.png" });
            request.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15, StoragePath = "test2/test2.png" });
            request.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20, StoragePath = "test3/test3.png" });

            var response = client.Optimize(
                request
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [Test]
        public void Client_ImageSetUrlWaitAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var dataStore = new DataStore(
                  Settings.AmazonKey,
                  Settings.AmazonSecret,
                  Settings.AmazonBucket,
                  string.Empty
                  );

            var request = new OptimizeSetWaitRequest(new Uri(TestData.ImageOne), dataStore)
            {
                Lossy = true,
            };
            request.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10, StoragePath = "test1/test1.png" });
            request.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15, StoragePath = "test2/test2.png" });
            request.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20, StoragePath = "test3/test3.png" });

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

        [Test]
        public void Client_ImageSetUploadWaitAmazon_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var dataStore = new DataStore(
                  Settings.AmazonKey,
                  Settings.AmazonSecret,
                  Settings.AmazonBucket,
                  string.Empty
                  );

            var request = new OptimizeSetUploadWaitRequest(dataStore)
            {
                Lossy = true,
            };
            request.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10, StoragePath = "test1/test1.png" });
            request.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15, StoragePath = "test2/test2.png" });
            request.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20, StoragePath = "test3/test3.png" });

            var response = client.OptimizeWait(GetPathResources(TestData.LocalTestImage),
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
    }
}
