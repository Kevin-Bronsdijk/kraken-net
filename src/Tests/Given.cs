using System;
using Kraken;
using Kraken.Http;
using Kraken.Model;

namespace Tests
{
    public static class Given
    {
        public static string AFilePathOnDisk => "";

        public static Uri ACallBackUrl => new Uri("http://initial.url");

        public static Uri AnExternalImageUrl => new Uri("http://initial.url");

        public static Client AClient => new Client(AConnection);

        public static OptimizeWaitRequest AOptimizeWaitRequest => new OptimizeWaitRequest(null);

        public static OptimizeRequest AOptimizeRequest => new OptimizeRequest(AnExternalImageUrl, ACallBackUrl);

        public static Connection AConnection => Connection.Create("init", "init");

        public static OptimizeUploadWaitRequest AOptimizeUploadWaitRequest => new OptimizeUploadWaitRequest();

        public static OptimizeUploadRequest AOptimizeUploadRequest => new OptimizeUploadRequest(ACallBackUrl);

        public static OptimizeSetUploadRequest AOptimizeSetUploadRequest => new OptimizeSetUploadRequest(ACallBackUrl);

        public static ResizeImageSet AResizeImageSet => new ResizeImageSet();
    }
}
