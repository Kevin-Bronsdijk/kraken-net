using System;
using System.IO;
using Kraken;
using Kraken.Http;
using Kraken.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ConnectionCreate_EmptyKeyError_IsTrue()
        {
            try
            {
                Connection.Create("", "secret");
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Argument must not be the empty string.\r\nParameter name: apiKey");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ConnectionCreate_NullKeyError_IsTrue()
        {
            try
            {
                Connection.Create(null, "secret");
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null.\r\nParameter name: apiKey");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ConnectionCreate_EmptySecretError_IsTrue()
        {
            try
            {
                Connection.Create("key", "");
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Argument must not be the empty string.\r\nParameter name: apiSecret");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void ConnectionCreate_NullSecretError_IsTrue()
        {
            try
            {
                Connection.Create("key", null);
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null.\r\nParameter name: apiSecret");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_NullConnectionError_IsTrue()
        {
            try
            {
                var client = new Client(null);
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null.\r\nParameter name: connection");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_NoErrors_IsTrue()
        {
            try
            {
                var connection = Connection.Create("key", "secret");
                var client = new Client(connection);

                Assert.IsTrue(client != null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_IsSandboxMode_IsTrue()
        {
            var connection = Connection.Create("key", "secret", true);

            Assert.IsTrue(connection.SandboxMode);
        }

        [TestMethod]
        public void Client_NotInSandboxMode_IsTrue()
        {
            var connection = Connection.Create("key", "secret");

            Assert.IsTrue(connection.SandboxMode == false);
        }

        [TestMethod]
        public void Client_NotInSandboxModeExpl_IsTrue()
        {
            var connection = Connection.Create("key", "secret", false);

            Assert.IsTrue(connection.SandboxMode == false);
        }

        [TestMethod]
        public void ConnectionCreate_Dispose_IsTrue()
        {
            var connection = Connection.Create("key", "secret");

            try
            {
                connection.Dispose();

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_MustProvideAConnection_IsTrue()
        {
            try
            {
                var client = new Client(null);

                Assert.IsTrue(false, "No exception");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null.\r\nParameter name: connection");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_Dispose_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            try
            {
                client.Dispose();

                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false, "Exception");
            }
        }

        [TestMethod]
        public void Client_RequestUploadWaitNoFileNameError_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            try
            {
                client.OptimizeWait(
                    null,
                    string.Empty,
                    new OptimizeUploadWaitRequest()
                    );

                Assert.IsTrue(false, "No exception");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Value cannot be null.\r\nParameter name: image");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_RequestUploadCallbackNoFileNameError_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            try
            {
                client.Optimize(
                    null,
                    string.Empty,
                    new OptimizeUploadRequest(new Uri("http://kraken.io/test"))
                    );

                Assert.IsTrue(false, "No exception");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message == "Argument must not be the empty string.\r\nParameter name: filename");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_RequestUploadWaitNoFile_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            try
            {
                client.OptimizeWait("z:\\test\\test.jpg",
                    new OptimizeUploadWaitRequest()
                    );

                Assert.IsTrue(false, "No exception");
            }
            catch (FileNotFoundException ex)
            {
                Assert.IsTrue(ex.Message == "Unable to find the specified file.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_RequestUploadNoFile_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            try
            {
                client.Optimize("z:\\test\\test.jpg",
                    new OptimizeUploadRequest(new Uri("http://kraken.io/test"))
                    );

                Assert.IsTrue(false, "No exception");
            }
            catch (FileNotFoundException ex)
            {
                Assert.IsTrue(ex.Message == "Unable to find the specified file.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_RequestUploadWaitNoFileCompressionDefaults_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            try
            {
                client.OptimizeWait("z:\\test\\test.jpg");

                Assert.IsTrue(false, "No exception");
            }
            catch (FileNotFoundException ex)
            {
                Assert.IsTrue(ex.Message == "Unable to find the specified file.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void Client_RequestUploadNoFileCompressionDefaults_IsTrue()
        {
            var connection = Connection.Create("key", "secret");
            var client = new Client(connection);

            try
            {
                client.Optimize("z:\\test\\test.jpg", new Uri("http://kraken.io/test"));

                Assert.IsTrue(false, "No exception");
            }
            catch (FileNotFoundException ex)
            {
                Assert.IsTrue(ex.Message == "Unable to find the specified file.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }

        [TestMethod]
        public void OptimizeSetRequestBase_ImageSetSameName_IsTrue()
        {
            var optimizeSetUploadRequest = new OptimizeSetUploadRequest(new Uri("http://kraken.io/test"));

            var Sri = new ResizeImageSet { Name = "test1", Height = 10, Width = 10 };

            optimizeSetUploadRequest.AddSet(Sri);

            try
            {
                optimizeSetUploadRequest.AddSet(Sri);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "Item already exists in collection");
            }
        }

        [TestMethod]
        public void OptimizeSetRequestBase_ImageSetexceedMaximum_IsTrue()
        {
            var optimizeSetUploadRequest = new OptimizeSetUploadRequest(new Uri("http://kraken.io/test"));

            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test2", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test3", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test4", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test5", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test6", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test7", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test8", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test9", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test10", Height = 10, Width = 10 });

            try
            {
                optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "error", Height = 10, Width = 10 });
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "Cannot exceed the quota of 10 instructions per request");
            }
        }

        [TestMethod]
        public void General_isUri_IsTrue()
        {
            // Just wanted to see if this is a valid Uri as per
            // https://kraken.io/account/updates
            var HttpBasicAuthentication = new Uri("https://username:password@awesome-website.com/images/header.png");
        }
    }
}