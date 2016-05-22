namespace Kraken.Http
{
    public interface IApiResponse<out TResult> : IApiResponse
    {
        TResult Body { get; }
    }
}