namespace Kraken.Model
{
    public interface IOptimizeSetWaitRequest : IRequest
    {
        void AddSet(ResizeImageSet setResizeImage);
    }
}
