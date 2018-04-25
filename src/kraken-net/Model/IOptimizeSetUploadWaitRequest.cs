namespace Kraken.Model
{
    public interface IOptimizeSetUploadWaitRequest : IRequest
    {
        void AddSet(ResizeImageSet setResizeImage);
    }
}
