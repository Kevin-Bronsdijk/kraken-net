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

    public static class OptimizeRequestExt
    {
        public static OptimizeRequest ThatHasAnExistingImageAndValidCallBackUri(this OptimizeRequest optimizeRequest)
        {
            optimizeRequest = new OptimizeRequest(
                Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb()
                , Given.ACallBackUrl.ThatIsAValidCallBackUrl());

            return optimizeRequest;
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
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                Lossy = true
            };

            return optimizeWaitRequest;
        }

        public static OptimizeWaitRequest ThatSetsTheImageFormatToGif(this OptimizeWaitRequest optimizeWaitRequest)
        {
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                ConvertImage = new ConvertImage(ImageFormat.Gif)
            };

            return optimizeWaitRequest;
        }

        public static OptimizeWaitRequest ThatConvertsTheImageToGifWithABackgroundColor(this OptimizeWaitRequest optimizeWaitRequest)
        {
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
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
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
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

        public static OptimizeWaitRequest ThatResizesTheImageIntoASquare(this OptimizeWaitRequest optimizeWaitRequest)
        {
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                ResizeImage = new ResizeImage
                {
                    Size = 120,
                    Strategy = Strategy.Square,
                }
            };

            return optimizeWaitRequest;
        }

        public static OptimizeWaitRequest ThatSetsCustomQuality(this OptimizeWaitRequest optimizeWaitRequest)
        {
            optimizeWaitRequest = new OptimizeWaitRequest(new Uri(TestData.ImageOne))
            {
                Lossy = true,
                Quality = 90
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

        public static OptimizeUploadWaitRequest ThatHasResizeOptions(this OptimizeUploadWaitRequest optimizeUploadWaitRequest)
        {
            return new OptimizeUploadWaitRequest
            {
                ResizeImage = new ResizeImage {Height = 100, Width = 100},
                WebP = true
            };
        }

        public static OptimizeUploadWaitRequest ThatHasLossyWebPAndSamplingScheme(this OptimizeUploadWaitRequest optimizeUploadWaitRequest)
        {
            return new OptimizeUploadWaitRequest
            {
                Lossy = true,
                WebP = true,
                SamplingScheme = SamplingScheme.S444
            };
        }

        public static OptimizeUploadWaitRequest ThatHasAutoOrientOn(this OptimizeUploadWaitRequest optimizeUploadWaitRequest)
        {
            return new OptimizeUploadWaitRequest
            {
                AutoOrient = true
            };
        }
    }

    public static class OptimizeUploadRequestExt
    {
        public static OptimizeUploadRequest ThatHasAValidCallbackUrl(this OptimizeUploadRequest optimizeUploadRequest)
        {
            return new OptimizeUploadRequest(Given.ACallBackUrl.ThatIsAValidCallBackUrl());
        }

        public static OptimizeUploadRequest ThatHasResizeOptions(this OptimizeUploadRequest optimizeUploadRequest)
        {
            return new OptimizeUploadRequest(Given.ACallBackUrl.ThatIsAValidCallBackUrl())
            {
                ResizeImage = new ResizeImage {Height = 100, Width = 100},
                WebP = true
            };
        }
    }

    public static class OptimizeSetRequestExt
    {
        public static OptimizeSetRequest ThatHasASetOf3(this OptimizeSetRequest optimizeSetRequest)
        {
            optimizeSetRequest = new OptimizeSetRequest(Given.ACallBackUrl.ThatPointsToAValidImageOnTheWeb(), Given.ACallBackUrl.ThatIsAValidCallBackUrl())
            {
                Lossy = true
            };

            optimizeSetRequest.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10 });
            optimizeSetRequest.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15 });
            optimizeSetRequest.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20 });

            return optimizeSetRequest;
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


        public static OptimizeSetUploadRequest ThatHasASetOf3(this OptimizeSetUploadRequest optimizeSetUploadRequest)
        {
            optimizeSetUploadRequest = new OptimizeSetUploadRequest(Given.ACallBackUrl.ThatIsAValidCallBackUrl())
            {
                Lossy = true
            };

            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15 });
            optimizeSetUploadRequest.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20 });

            return optimizeSetUploadRequest;
        }
    }

    public static class OptimizeSetWaitRequestExt
    {
        public static OptimizeSetWaitRequest ThatHasASetOf3(this OptimizeSetWaitRequest optimizeSetWaitRequest)
        {
            optimizeSetWaitRequest = new OptimizeSetWaitRequest(Given.AnExternalImageUrl.ThatPointsToAValidImageOnTheWeb())
            {
                Lossy = true
            };

            optimizeSetWaitRequest.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10 });
            optimizeSetWaitRequest.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15 });
            optimizeSetWaitRequest.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20 });

            return optimizeSetWaitRequest;
        }
    }

    public static class OptimizeSetUploadWaitRequestExt
    {
        public static OptimizeSetUploadWaitRequest ThatHasASetOf3(this OptimizeSetUploadWaitRequest optimizeSetUploadWaitRequest)
        {
            optimizeSetUploadWaitRequest = new OptimizeSetUploadWaitRequest()
            {
                Lossy = true
            };

            optimizeSetUploadWaitRequest.AddSet(new ResizeImageSet { Name = "test1", Height = 10, Width = 10 });
            optimizeSetUploadWaitRequest.AddSet(new ResizeImageSet { Name = "test2", Height = 15, Width = 15 });
            optimizeSetUploadWaitRequest.AddSet(new ResizeImageSet { Name = "test3", Height = 20, Width = 20 });

            return optimizeSetUploadWaitRequest;
        }

        public static OptimizeSetUploadWaitRequest ThatHasASetOf2WithCustomSettings(this OptimizeSetUploadWaitRequest optimizeSetUploadWaitRequest)
        {
            optimizeSetUploadWaitRequest = new OptimizeSetUploadWaitRequest()
            {
                Lossy = true
            };
            optimizeSetUploadWaitRequest.AddSet(new ResizeImageSet
            {
                Name = "test1",
                Height = 10,
                Width = 10,
                Lossy = false
            });
            optimizeSetUploadWaitRequest.AddSet(new ResizeImageSet
            {
                Name = "test2",
                Height = 15,
                Width = 15,
                SamplingScheme = SamplingScheme.S444
            });
            return optimizeSetUploadWaitRequest;
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
