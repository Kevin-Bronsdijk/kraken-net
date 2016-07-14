using Kraken.Model;
using System;
using Tests;

namespace WebTest.TestLogic
{
    public class AzureTests
    {
        public static bool CanCreateClient()
        {
            try
            {
                var client = HelperFunctions.CreateWorkingClient();
                return (client != null);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CanUploadImage(string imagePath)
        {
            try
            {
                var client = HelperFunctions.CreateWorkingClient();
                var image = System.IO.File.ReadAllBytes(imagePath);

                var response = client.OptimizeWait(
                    image,
                    TestData.TestImageName,
                    new Kraken.Model.Azure.OptimizeUploadWaitRequest(
                        Settings.AzureAccount,
                        Settings.AzureKey,
                        Settings.AzureContainer,
                        "/test/" + TestData.TestImageName
                        )
                    );

                return response.Result.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}