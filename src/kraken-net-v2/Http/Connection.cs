using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Logic;
using Kraken.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kraken.Http
{
    public class Connection : IDisposable
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly Uri _krakenApiUrl = new Uri("https://api.kraken.io/");
        private HttpClient _client;
        private JsonSerializerSettings _serializerSettings;

        internal Connection(string apiKey, string apiSecret, HttpMessageHandler handler, bool sandboxMode)
        {
            _client = new HttpClient(handler) { BaseAddress = _krakenApiUrl };
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.85 Safari/537.36");

            SandboxMode = sandboxMode;

            ConfigureSerialization();

            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public bool SandboxMode { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void ConfigureSerialization()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCaseExceptDictionaryKeysResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter { CamelCaseText = true } },
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public static Connection Create(string apiKey, string apiSecret, IWebProxy proxy = null)
        {
            apiKey.ThrowIfNullOrEmpty("apiKey");
            apiSecret.ThrowIfNullOrEmpty("apiSecret");

            var handler = new HttpClientHandler { Proxy = proxy };
            return new Connection(apiKey, apiSecret, handler, false);
        }

        public static Connection Create(string apiKey, string apiSecret, bool sandboxMode, IWebProxy proxy = null)
        {
            apiKey.ThrowIfNullOrEmpty("apiKey");
            apiSecret.ThrowIfNullOrEmpty("apiSecret");

            var handler = new HttpClientHandler { Proxy = proxy };
            return new Connection(apiKey, apiSecret, handler, sandboxMode);
        }

        internal async Task<IApiResponse<TResponse>> Execute<TResponse>(ApiRequest apiRequest,
            CancellationToken cancellationToken)
        {
            apiRequest.Body.Authentication.ApiKey = _apiKey;
            apiRequest.Body.Authentication.ApiSecret = _apiSecret;
            apiRequest.Body.Dev = SandboxMode || apiRequest.Body.Dev;
            var isSet = apiRequest.Body is IOptimizeSetWaitRequest || apiRequest.Body is IOptimizeSetUploadWaitRequest;

            using (var requestMessage = new HttpRequestMessage(apiRequest.Method, apiRequest.Uri))
            {
                var json = JsonConvert.SerializeObject(apiRequest.Body, _serializerSettings);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

                using (
                    var responseMessage =
                        await _client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false))
                {
                    return await BuildResponse<TResponse>(responseMessage, isSet, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        internal async Task<IApiResponse<TResponse>> ExecuteUpload<TResponse>(
            ApiRequest apiRequest, byte[] image, string filename, CancellationToken cancellationToken)
        {
            filename.ThrowIfNullOrEmpty("filename");

            apiRequest.Body.Authentication.ApiKey = _apiKey;
            apiRequest.Body.Authentication.ApiSecret = _apiSecret;
            apiRequest.Body.Dev = SandboxMode || apiRequest.Body.Dev;
            var isSet = apiRequest.Body is IOptimizeSetWaitRequest || apiRequest.Body is IOptimizeSetUploadWaitRequest;

            using (
                var content =
                    new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                var json = JsonConvert.SerializeObject(apiRequest.Body, _serializerSettings);
                content.Add(new StringContent(json, Encoding.UTF8, "application/json"), "data");

                content.Add(new StreamContent(new MemoryStream(image)), filename, filename);

                using (
                    var responseMessage =
                        await _client.PostAsync(_krakenApiUrl + apiRequest.Uri, content, cancellationToken).ConfigureAwait(false))
                {
                    return await BuildResponse<TResponse>(responseMessage, isSet, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        private async Task<T> ParseResponseMessageToObject<T>(HttpResponseMessage responseMessage,
            CancellationToken cancellationToken)
        {
            using (var stream = await responseMessage.Content.ReadAsStreamAsync())
            {
                //Todo: Implement cancellationToken support
                //await stream.CopyToAsync(responseStream2, 4096, cancellationToken);
                return JsonConvert.DeserializeObject<T>(new StreamReader(stream).ReadToEnd(), _serializerSettings);
            }
        }

        private async Task<IApiResponse<TResponse>> BuildResponse<TResponse>(HttpResponseMessage message, bool isSet,
            CancellationToken cancellationToken)
        {
            var response = new ApiResponse<TResponse>
            {
                StatusCode = message.StatusCode,
                Success = message.IsSuccessStatusCode
            };

            if (message.Content != null)
            {
                if (message.IsSuccessStatusCode)
                {
                    if (isSet)
                    {
                        // Manual parsing required
                        var json = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

                        //Todo: Refactor later (formatter)
                        var optimizeSetWaitResults = Helper.JsonToSet(json);

                        response.Body = (TResponse)(object)optimizeSetWaitResults;
                    }
                    else
                    {
                        response.Body = await ParseResponseMessageToObject<TResponse>(message, cancellationToken).ConfigureAwait(false);
                    }
                }
                else
                {
                    var errorResponse =
                        await
                            ParseResponseMessageToObject<ErrorResult>(message, cancellationToken).ConfigureAwait(false);

                    if (errorResponse != null)
                    {
                        response.Error = errorResponse.Error;
                    }
                }
            }

            return response;
        }

        ~Connection()
        {
            Dispose(false);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
            }
        }
    }
}