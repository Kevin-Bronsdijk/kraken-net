using Kraken.Model;
using System;
using Tests;

namespace WebTest.TestLogic
{
    public class Tests
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
            var client = HelperFunctions.CreateWorkingClient();

            try
            {
                var image = System.IO.File.ReadAllBytes(imagePath);

                var response = client.OptimizeWait(image, TestData.TestImageName,
                    new OptimizeUploadWaitRequest
                    {
                        ResizeImage = new ResizeImage { Height = 100, Width = 100 },
                        WebP = true
                    });

                return response.Result.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}