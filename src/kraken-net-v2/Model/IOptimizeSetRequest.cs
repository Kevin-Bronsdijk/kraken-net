
namespace Kraken.Model
{
    public interface IOptimizeSetRequest : IRequest
    {
        void AddSet(ResizeImageSet setResizeImage);
    }
}
