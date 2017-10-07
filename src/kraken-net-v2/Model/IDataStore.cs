namespace Kraken.Model
{
    public interface IDataStore
    {
        void AddMetadata(string key, string value);

        void AddHeaders(string key, string value);
    }
}