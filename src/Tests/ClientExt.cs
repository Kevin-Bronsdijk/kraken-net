using System;
using Kraken;
using Kraken.Http;
using Kraken.Model;

namespace Tests
{
    public static class ClientExt
    {
        public static Client ThatCanConnect(this Client client, bool debug = false)
        {
            var connection = Given.AConnection.ThatCanConnectToLive();
            return new Client(connection);
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
    }

    public static class ConnectionExt
    {
        public static Connection ThatCanConnectToLive(this Connection connection, bool debug = false)
        {
            return Connection.Create(Settings.ApiKey, Settings.ApiSecret, debug);
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
