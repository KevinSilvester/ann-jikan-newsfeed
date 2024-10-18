namespace ANN_Jikan.Services
{
    public interface IResponseParser<T>
    {
        T Parse(string response);
    }
}
