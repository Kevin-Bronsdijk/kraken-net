using System;
using Kraken;
using Kraken.Http;
using Kraken.Model;

namespace Tests
{
    public static class Given
    {
        public static Client AClient => new Client(AConnection);

        public static OptimizeWaitRequest AOptimizeWaitRequest => new OptimizeWaitRequest(null);

        public static Connection AConnection => Connection.Create("init", "init");
    }
}
