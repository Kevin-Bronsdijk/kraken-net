using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Http;
using Kraken.Logic;
using Kraken.Model;

namespace Kraken
{
    public class Client : IDisposable
    {
        private Connection _connection;

        public Client(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<IApiResponse<UserResult>> UserStatus(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var userRequest = new UserRequest();
            var message = _connection.Execute<UserResult>(new ApiRequest(userRequest, "user_status"),
                cancellationToken);

            return message;
        }

        #region Simple Requests

        // Keep all default settings as provided by Kraken 

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(Uri imageUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (imageUri == null)
            {
                throw new ArgumentNullException(nameof(imageUri));
            }

            var optimizeRequest = new OptimizeWaitRequest(imageUri);

            var message = OptimizeWait(optimizeRequest, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(Uri imageUri, Uri callbackUrl,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (imageUri == null)
            {
                throw new ArgumentNullException(nameof(imageUri));
            }
            if (callbackUrl == null)
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }

            var optimize = new OptimizeRequest(imageUri, callbackUrl);

            var message = Optimize(optimize, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(string filePath, CancellationToken cancellationToken = default(CancellationToken))
        {
            return OptimizeWait(filePath, new OptimizeUploadWaitRequest(), cancellationToken);
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(string filePath, Uri callbackUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Optimize(filePath, new OptimizeUploadRequest(callbackUrl), cancellationToken);
        }

        #endregion

        #region Custom Requests 

        // Requests that can be customized by providing a custom request

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(string filePath,
            IOptimizeUploadWaitRequest optimizeWaitRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            filePath.ThrowIfNullOrEmpty("filePath");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            if (optimizeWaitRequest == null)
            {
                throw new ArgumentNullException(nameof(optimizeWaitRequest));
            }
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var file = File.ReadAllBytes(filePath);

            var message =
                _connection.ExecuteUpload<OptimizeWaitResult>(new ApiRequest(optimizeWaitRequest, "v1/upload"),
                    file, Path.GetFileName(filePath), cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(string filePath,
            IOptimizeUploadRequest optimizeRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            filePath.ThrowIfNullOrEmpty("filePath");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            if (optimizeRequest == null)
            {
                throw new ArgumentNullException(nameof(optimizeRequest));
            }
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var file = File.ReadAllBytes(filePath);

            var message = _connection.ExecuteUpload<OptimizeResult>(new ApiRequest(optimizeRequest, "v1/upload"),
                file, filePath, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(byte[] image, string filename,
            IOptimizeUploadWaitRequest optimizeWaitRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }
            filename.ThrowIfNullOrEmpty("filename");

            var message =
                _connection.ExecuteUpload<OptimizeWaitResult>(new ApiRequest(optimizeWaitRequest, "v1/upload"),
                    image, filename, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(byte[] image, string filename,
            IOptimizeUploadRequest optimizeRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            filename.ThrowIfNullOrEmpty("filename");
            if (optimizeRequest == null)
            {
                throw new ArgumentNullException(nameof(optimizeRequest));
            }
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.ExecuteUpload<OptimizeResult>(new ApiRequest(optimizeRequest, "v1/upload"),
                image, filename, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeWaitResult>> OptimizeWait(IOptimizeWaitRequest optimizeWaitRequest,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (optimizeWaitRequest == null)
            {
                throw new ArgumentNullException(nameof(optimizeWaitRequest));
            }
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.Execute<OptimizeWaitResult>(new ApiRequest(optimizeWaitRequest, "v1/url"),
                cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(IOptimizeRequest optimizeRequest,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (optimizeRequest == null)
            {
                throw new ArgumentNullException(nameof(optimizeRequest));
            }
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var message = _connection.Execute<OptimizeResult>(new ApiRequest(optimizeRequest, "v1/url"),
                cancellationToken);

            return message;
        }

        #endregion

        #region Image Sets

        public Task<IApiResponse<OptimizeSetWaitResults>> OptimizeWait(IOptimizeSetWaitRequest optimizeSetWaitRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var message = _connection.Execute<OptimizeSetWaitResults>(new ApiRequest(optimizeSetWaitRequest, "v1/url"),
                cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(IOptimizeSetRequest optimizeSetRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var message = _connection.Execute<OptimizeResult>(new ApiRequest(optimizeSetRequest, "v1/url"),
                cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeSetWaitResults>> OptimizeWait(byte[] image, string filename,
            IOptimizeSetUploadWaitRequest optimizeWaitRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            filename.ThrowIfNullOrEmpty("filename");

            var message =
                _connection.ExecuteUpload<OptimizeSetWaitResults>(new ApiRequest(optimizeWaitRequest, "v1/upload"),
                    image, filename, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeSetWaitResults>> OptimizeWait(string filePath,
            IOptimizeSetUploadWaitRequest optimizeWaitRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            filePath.ThrowIfNullOrEmpty("filePath");
            if (!File.Exists(filePath)) { throw new FileNotFoundException(); }

            var file = File.ReadAllBytes(filePath);

            var message =
                _connection.ExecuteUpload<OptimizeSetWaitResults>(new ApiRequest(optimizeWaitRequest, "v1/upload"),
                    file, Path.GetFileName(filePath), cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(byte[] image, string filename,
            IOptimizeSetUploadRequest optimizeRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            filename.ThrowIfNullOrEmpty("filename");

            var message = _connection.ExecuteUpload<OptimizeResult>(new ApiRequest(optimizeRequest, "v1/upload"),
                image, filename, cancellationToken);

            return message;
        }

        public Task<IApiResponse<OptimizeResult>> Optimize(string filePath,
            IOptimizeSetUploadRequest optimizeRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            filePath.ThrowIfNullOrEmpty("filePath");
            if (!File.Exists(filePath)) { throw new FileNotFoundException(); }

            var file = File.ReadAllBytes(filePath);

            var message = _connection.ExecuteUpload<OptimizeResult>(new ApiRequest(optimizeRequest, "v1/upload"),
                file, filePath, cancellationToken);

            return message;
        }

        #endregion

        ~Client()
        {
            Dispose(false);
        }

        internal virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
    }
}