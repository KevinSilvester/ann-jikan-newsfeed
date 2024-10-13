using System.Text.Json;
using RestSharp;

namespace ANN.Jikan
{
    public class Jikan
    {
        private static RestClient ApiClient = new RestClient("https://api.jikan.moe/v4/anime");
    }
}
