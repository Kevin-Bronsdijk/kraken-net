namespace Kraken.Model
{
    public interface IRequest
    {
        Authentication Authentication { get;  }
        bool Dev { get; set; }
    }
}