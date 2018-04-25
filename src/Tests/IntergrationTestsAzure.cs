using System;
using System.IO;
using System.Net;
using System.Reflection;
using Kraken.Model;
using Kraken.Model.Azure;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using OptimizeRequest = Kraken.Model.Azure.OptimizeRequest;
using OptimizeUploadRequest = Kraken.Model.Azure.OptimizeUploadRequest;
using OptimizeUploadWaitRequest = Kraken.Model.Azure.OptimizeUploadWaitRequest;
using OptimizeWaitRequest = Kraken.Model.Azure.OptimizeWaitRequest;
using OptimizeSetRequest = Kraken.Model.Azure.OptimizeSetRequest;
using OptimizeSetUploadRequest = Kraken.Model.Azure.OptimizeSetUploadRequest;
using OptimizeSetUploadWaitRequest = Kraken.Model.Azure.OptimizeSetUploadWaitRequest;
using OptimizeSetWaitRequest = Kraken.Model.Azure.OptimizeSetWaitRequest;

namespace Tests
{
    [TestFixture]
    [NUnit.Framework.Ignore("Ignore for CI")]
    public class IntergrationTestsAzure
    {
        private static string GetPathResources(string nameResourse)
        {
            var path = Path.GetDirectoryName(path: new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            return $"{path}\\images\\{nameResourse}";
        }
        
        // Not checking the results of the webhooks
        private readonly Uri _callbackUri = new Uri("http://requestb.in/15gm5dz1");

        [Test]
        public void Client_OptimizeWaitAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                new Uri(TestData.ImageOne),
                 new DataStore(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [Test]
        public void Client_OptimizeCallbackAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Optimize(
                new OptimizeRequest(
                new Uri(TestData.ImageOne),
                _callbackUri,
                 new DataStore(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [Test]
        public void Client_OptimizeCallbackAzureParams_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Optimize(
                new OptimizeRequest(
                new Uri(TestData.ImageOne),
                _callbackUri,
                Settings.AzureAccount,
                Settings.AzureKey,
                Settings.AzureContainer
                ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [Test]
        public void Client_OptimizeCallbackAzureParamsAndPath_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.Optimize(
                new OptimizeRequest(
                new Uri(TestData.ImageOne),
                _callbackUri,
                Settings.AzureAccount,
                Settings.AzureKey,
                Settings.AzureContainer,
                "/test123/"
                ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [Test]
        public void Client_UploadOptimizeWaitAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

            var response = client.OptimizeWait(
                image,
                TestData.TestImageName,
                new OptimizeUploadWaitRequest(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [Test]
        public void Client_UploadOptimizeWaitAzureDataStore_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

            var response = client.OptimizeWait(
                image,
                TestData.TestImageName,
                new OptimizeUploadWaitRequest(
                    new DataStore(
                        Settings.AzureAccount,
                        Settings.AzureKey,
                        Settings.AzureContainer)
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [Test]
        public void Client_UploadOptimizeWaitAzureWithPath_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

            var response = client.OptimizeWait(
                image,
                TestData.TestImageName,
                new OptimizeUploadWaitRequest(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer,
                    "/test/" + TestData.TestImageName
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
            // Check path
        }

        [Test]
        public void Client_UploadOptimizeCallbackAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

            var response = client.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest(
                    _callbackUri,
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [Test]
        public void Client_UploadOptimizeCallbackAzureDataStore_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

            var response = client.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest(
                    _callbackUri,
                    new DataStore(
                        Settings.AzureAccount,
                        Settings.AzureKey,
                        Settings.AzureContainer)
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
        }

        [Test]
        public void Client_UploadOptimizeCallbackAzureWithPath_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

            var response = client.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest(
                    _callbackUri,
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer,
                    "/test/" + TestData.TestImageName
                    )
                );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Body != null);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.Id));
            // Check path
        }

        [Test]
        public void Client_CustomRequestUploadWaitAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

            var response = client.OptimizeWait(
                image,
                TestData.TestImageName,
                new OptimizeUploadWaitRequest(
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
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
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [Test]
        public void Client_CustomRequestUploadCallbackAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var image = File.ReadAllBytes(GetPathResources(TestData.LocalTestImage));

            var response = client.Optimize(
                image,
                TestData.TestImageName,
                new OptimizeUploadRequest(
                    _callbackUri,
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    )
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

        [Test]
        public void Client_OptimizeWaitAzureUsingIOptimizeWaitRequest_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [Test]
        public void Client_OptimizeWaitAzureUsingIOptimizeWaitRequestWithPath_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    Settings.AzureContainer,
                    "/test/" + TestData.TestImageName
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [Test]
        public void Client_OptimizeWaitAzureUsingIOptimizeWaitRequestDataStore_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    new DataStore(
                        Settings.AzureAccount,
                        Settings.AzureKey,
                        Settings.AzureContainer)
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }
        
        [Test]
        public void Client_OptimizeWaitAzureUsingIOptimizeWaitRequestWithRootPath_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    Settings.AzureAccount,
                    Settings.AzureKey,
                    "$root",
                    TestData.TestImageName
                    ));

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }
        
        [Test]
        public void Client_OptimizeWaitAzureAddHeadersAndMeta_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();

            var dataStore = new DataStore(
                Settings.AzureAccount,
                Settings.AzureKey,
                Settings.AzureContainer);

            dataStore.AddMetadata("x-ms-meta-test1", "value1"); // Prefix will be removed and added by kraken later
            dataStore.AddMetadata("test2", "value2");
            dataStore.AddHeaders("Cache-Control", "public, max-age=2222");

            var response = client.OptimizeWait(
                new OptimizeWaitRequest(
                    new Uri(TestData.ImageOne),
                    dataStore
                    )
                {
                    ResizeImage = new ResizeImage { Height = 100, Width = 100, Enhance = false},
                    WebP = true
                }
            );

            var result = response.Result;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Body.KrakedUrl));
            Assert.IsTrue(result.Body.KrakedUrl.Contains("blob.core.windows.net"));
        }

        [Test]
        public void Client_ImageSetUploadCallBackAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var dataStore = new DataStore(
                Settings.AzureAccount,
                Settings.AzureKey,
                Settings.AzureContainer);

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
        public void Client_ImageSetUrlCallBackAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var dataStore = new DataStore(
                Settings.AzureAccount,
                Settings.AzureKey,
                Settings.AzureContainer);

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
        public void Client_ImageSetUrlWaitAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var dataStore = new DataStore(
                Settings.AzureAccount,
                Settings.AzureKey,
                Settings.AzureContainer);

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
        public void Client_ImageSetUploadWaitAzure_IsTrue()
        {
            var client = HelperFunctions.CreateWorkingClient();
            var dataStore = new DataStore(
                Settings.AzureAccount,
                Settings.AzureKey,
                Settings.AzureContainer);

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
