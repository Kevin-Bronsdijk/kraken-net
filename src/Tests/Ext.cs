using System;
using Kraken;
using Kraken.Http;
using Kraken.Model;

namespace Tests
{
    public static class Ext
    {
        public static Client ThatCanConnect(this Client client, bool debug = false)
        {
            var connection = Given.AConnection.ThatCanConnectToLive(debug);
            return new Client(connection);
        }

        public static Client ThatDoesntHaveAConnection(this Client client, bool debug = false)
        {
            return new Client(null);
        }

        public static Client ThatHasAValidConnection(this Client client, bool debug = false)
        {
            var connection = Given.AConnection.ThatHasAValidArguments();
            return new Client(connection);
        }
    }

    public static class UriEx
    {
        public static Uri ThatIsAValidCallBackUrl(this Uri uri)
        {
            return new Uri("http://kraken.io/test");
        }

        public static Uri ThatPointsToAValidImageOnTheWeb(this Uri uri)
        {
            return new Uri("https://kraken.io/assets/images/kraken-logotype.png");
        }

        public static Uri ThatReturns404ImageLocation(this Uri uri)
        {
            return new Uri("https://kraken.io/im-out-for-lunch.png");
        }
    }

    public static class AFilePathOnDiskExt
    {
        public static string ThatPointsToAFileNotOnDisk(this string aFilePathOnDisk)
        {
            return "z:\\test\\test.jpg";
        }
    }

    public static class OptimizeWaitRequestExt
    {
        public static OptimizeWaitRequest ThatHasAUriToAnImageWithGeoTags(this OptimizeWaitRequest optimizeWaitRequest, 
            PreserveMeta[] preserveMeta = null)
        {
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageGeoTag))
            { PreserveMeta = preserveMeta};

            return optimizeWaitRequest;
        }

        public static OptimizeWaitRequest ThatHasLossySetAsTrue(this OptimizeWaitRequest optimizeWaitRequest)
        {
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageGeoTag))
            {
                Lossy = true
            };

            return optimizeWaitRequest;
        }

        public static OptimizeWaitRequest ThatSetsTheImageFormatToGif(this OptimizeWaitRequest optimizeWaitRequest)
        {
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageGeoTag))
            {
                ConvertImage = new ConvertImage(ImageFormat.Gif)
            };

            return optimizeWaitRequest;
        }

        public static OptimizeWaitRequest ThatConvertsTheImageToGifWithABackgroundColor(this OptimizeWaitRequest optimizeWaitRequest)
        {
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageGeoTag))
            {
                ConvertImage = new ConvertImage()
                {
                    BackgroundColor = "#ffffff",
                    Format = ImageFormat.Gif
                }
            };

            return optimizeWaitRequest;
        }

        public static OptimizeWaitRequest ThatResizesTheImage(this OptimizeWaitRequest optimizeWaitRequest)
        {
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageGeoTag))
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

            return optimizeWaitRequest;
        }
    }

    public static class OptimizeUploadWaitRequestExt
    {
        public static OptimizeUploadWaitRequest ThatInitialOptimizeUploadWaitRequest(this OptimizeUploadWaitRequest optimizeUploadWaitRequest)
        {
            return new OptimizeUploadWaitRequest();
        }
    }

    public static class OptimizeUploadRequestExt
    {
        public static OptimizeUploadRequest ThatHasAValidCallbackUrl(this OptimizeUploadRequest optimizeUploadRequest)
        {
            return new OptimizeUploadRequest(Given.ACallBackUrl.ThatIsAValidCallBackUrl());
        }
    }

    public static class OptimizeSetUploadRequestExt
    {
        public static OptimizeSetUploadRequest ThatHasAValidCallbackUrl(this OptimizeSetUploadRequest optimizeSetUploadRequest)
        {
            return new OptimizeSetUploadRequest(Given.ACallBackUrl.ThatIsAValidCallBackUrl());
        }

        public static OptimizeSetUploadRequest ThatAddTheSameImageSetTwoTimes(this OptimizeSetUploadRequest optimizeSetUploadRequest)
        {
            optimizeSetUploadRequest = new OptimizeSetUploadRequest(Given.ACallBackUrl.ThatIsAValidCallBackUrl());
            optimizeSetUploadRequest.AddSet(Given.AResizeImageSet.ThatInitialResizeImageSet());
            optimizeSetUploadRequest.AddSet(Given.AResizeImageSet.ThatInitialResizeImageSet());

            return optimizeSetUploadRequest;
        }

        public static OptimizeSetUploadRequest ThatAddsOver10ImagesSets(this OptimizeSetUploadRequest optimizeSetUploadRequest)
        {
            optimizeSetUploadRequest = new OptimizeSetUploadRequest(Given.ACallBackUrl.ThatIsAValidCallBackUrl());

            for (var i = 0; i < 11; i++)
            {
                optimizeSetUploadRequest.AddSet(Given.AResizeImageSet.ThatReturnsAvalidImageSet($"image{i}"));
            }
        
            return optimizeSetUploadRequest;
        }
    }

    public static class ResizeImageSetExt
    {
        public static ResizeImageSet ThatInitialResizeImageSet(this ResizeImageSet resizeImageSet)
        {
            return new ResizeImageSet() { Name = "test1", Height = 10, Width = 10 };
        }

        public static ResizeImageSet ThatReturnsAvalidImageSet(this ResizeImageSet resizeImageSet, 
            string name, int height = 10, int width = 10)
        {
            return new ResizeImageSet() { Name = name, Height = height, Width = width };
        }
    }

    public static class ConnectionExt
    {
        public static Connection ThatCanConnectToLive(this Connection connection, bool debug = false)
        {
            return Connection.Create(Settings.ApiKey, Settings.ApiSecret, debug);
        }

        public static Connection ThatHasAValidArguments(this Connection connection, bool debug = false)
        {
            return Connection.Create("Key", "ApiSecret", debug);
        }

        public static Connection ThatCantConnectWithAnEmptyKeyValue(this Connection connection)
        {
            return Connection.Create("", "ApiSecret");
        }

        public static Connection ThatCantConnectWithANullKeyValue(this Connection connection)
        {
            return Connection.Create(null, "ApiSecret");
        }

        public static Connection ThatCantConnectWithAnEmptySecretValue(this Connection connection)
        {
            return Connection.Create("Key", "");
        }

        public static Connection ThatCantConnectWithANullSecretValue(this Connection connection)
        {
            return Connection.Create("Key", null);
        }
    }
}
