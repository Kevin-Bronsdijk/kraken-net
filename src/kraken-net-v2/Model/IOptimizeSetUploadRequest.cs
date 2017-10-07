
namespace Kraken.Model
{
    public interface IOptimizeSetUploadRequest : IRequest
    {
        void AddSet(ResizeImageSet setResizeImage);
    }
}
