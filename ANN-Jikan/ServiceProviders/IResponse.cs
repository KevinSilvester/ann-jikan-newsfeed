namespace ANN_Jikan.ServiceProviders
{
    public interface IResponseParser<T>
    {
        T Parse(string response);
    }
}
