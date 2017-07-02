using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ConnectionCreate_EmptyKeyError_IsTrue()
        {
            Should.Throw<ArgumentException>(() => Given.AConnection.ThatCantConnectWithAnEmptyKeyValue())
                .Message.ShouldBe("Argument must not be the empty string.\r\nParameter name: apiKey");
        }

        [TestMethod]
        public void ConnectionCreate_NullKeyError_IsTrue()
        {
            Should.Throw<ArgumentException>(() => Given.AConnection.ThatCantConnectWithANullKeyValue())
                .Message.ShouldBe("Value cannot be null.\r\nParameter name: apiKey");
        }

        [TestMethod]
        public void ConnectionCreate_EmptySecretError_IsTrue()
        {
            Should.Throw<ArgumentException>(() => Given.AConnection.ThatCantConnectWithAnEmptySecretValue())
                .Message.ShouldBe("Argument must not be the empty string.\r\nParameter name: apiSecret");
        }

        [TestMethod]
        public void ConnectionCreate_NullSecretError_IsTrue()
        {
            Should.Throw<ArgumentException>(() => Given.AConnection.ThatCantConnectWithANullSecretValue())
                .Message.ShouldBe("Value cannot be null.\r\nParameter name: apiSecret");
        }

        [TestMethod]
        public void Client_IsSandboxMode_IsTrue()
        {
            Given.AConnection.ThatHasAValidArguments(true)
                .SandboxMode.ShouldBeTrue();
        }

        [TestMethod]
        public void Client_NotInSandboxMode_IsTrue()
        {
            Given.AConnection.ThatHasAValidArguments()
                .SandboxMode.ShouldBeFalse();
        }

        [TestMethod]
        public void ConnectionCreate_Dispose_IsTrue()
        {
            Should.NotThrow(() => Given.AConnection.ThatHasAValidArguments()
            .Dispose());
        }

        [TestMethod]
        public void Client_MustProvideAConnection_IsTrue()
        {
            Should.Throw<ArgumentException>(() => Given.AClient.ThatDoesntHaveAConnection())
                .Message.ShouldBe("Value cannot be null.\r\nParameter name: connection");
        }

        [TestMethod]
        public void Client_Dispose_IsTrue()
        {
            Should.NotThrow(() => Given.AClient.ThatHasAValidConnection()
                .Dispose());
        }

        [TestMethod]
        public void Client_RequestUploadWaitNoFileNameError_IsTrue()
        {
            Should.Throw<ArgumentException>(() => Given.AClient.ThatHasAValidConnection().OptimizeWait(
                    null, string.Empty, Given.AOptimizeUploadWaitRequest.ThatInitialOptimizeUploadWaitRequest()))
                .Message.ShouldBe("Value cannot be null.\r\nParameter name: image");
        }

        [TestMethod]
        public void Client_RequestUploadCallbackNoFileNameError_IsTrue()
        {

            Should.Throw<ArgumentException>(() => Given.AClient.ThatHasAValidConnection().Optimize(
                    null, string.Empty, Given.AOptimizeUploadRequest.ThatHasAValidCallbackUrl()))
                .Message.ShouldBe("Argument must not be the empty string.\r\nParameter name: filename");
        }

        [TestMethod]
        public void Client_RequestUploadWaitNoFile_IsTrue()
        {       
            // mock io
            Should.Throw<FileNotFoundException>(() => Given.AClient.ThatHasAValidConnection().OptimizeWait(
                    Given.AFilePathOnDisk.ThatPointsToAFileNotOnDisk(), 
                    Given.AOptimizeUploadWaitRequest.ThatInitialOptimizeUploadWaitRequest()))
                .Message.ShouldBe("Unable to find the specified file.");
        }

        [TestMethod]
        public void Client_RequestUploadNoFile_IsTrue()
        {
            Should.Throw<FileNotFoundException>(() => Given.AClient.ThatHasAValidConnection().Optimize(
                    Given.AFilePathOnDisk.ThatPointsToAFileNotOnDisk(), 
                    Given.AOptimizeUploadRequest.ThatHasAValidCallbackUrl()))
                .Message.ShouldBe("Unable to find the specified file.");
        }

        [TestMethod]
        public void Client_RequestUploadWaitNoFileCompressionDefaults_IsTrue()
        {
            Should.Throw<FileNotFoundException>(() => Given.AClient.ThatHasAValidConnection().OptimizeWait(
                    Given.AFilePathOnDisk.ThatPointsToAFileNotOnDisk()))
                .Message.ShouldBe("Unable to find the specified file.");
        }

        [TestMethod]
        public void Client_RequestUploadNoFileCompressionDefaults_IsTrue()
        {
            Should.Throw<FileNotFoundException>(() => Given.AClient.ThatHasAValidConnection().Optimize(
                    Given.AFilePathOnDisk.ThatPointsToAFileNotOnDisk(), Given.ACallBackUrl.ThatIsAValidCallBackUrl()))
                .Message.ShouldBe("Unable to find the specified file.");
        }

        [TestMethod]
        public void OptimizeSetRequestBase_ImageSetSameName_IsTrue()
        {
            Should.Throw<Exception>(() => Given.AOptimizeSetUploadRequest.ThatAddTheSameImageSetTwoTimes())
                .Message.ShouldBe("Item already exists in collection");
        }

        [TestMethod]
        public void OptimizeSetRequestBase_ImageSetexceedMaximum_IsTrue()
        {
            Should.Throw<Exception>(() => Given.AOptimizeSetUploadRequest.ThatAddsOver10ImagesSets())
                .Message.ShouldBe("Cannot exceed the quota of 10 instructions per request");
        }
    }
}