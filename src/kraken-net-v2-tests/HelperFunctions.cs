using System;
using System.IO;
using System.Net;
using Kraken;
using Kraken.Http;

namespace Tests
{
    public static class HelperFunctions
    {
        public static Client CreateWorkingClient(bool debug = false)
        {
            var connection = Connection.Create(Settings.ApiKey, Settings.ApiSecret, debug);
            return new Client(connection);
        }

        //public static string DownloadImage(string fileLocation)
        //{
        //    var fileName = Path.GetTempPath() + Guid.NewGuid() + Path.GetFileName(fileLocation);

        //    using (var client = new WebClient())
        //    {
        //        client.DownloadFile(fileLocation, fileName);
        //    }

        //    return fileName;
        //}
    }
}