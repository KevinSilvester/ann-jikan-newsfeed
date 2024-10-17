using RestSharp;

namespace ANN_Jikan.ServiceProviders
{
    public class Client
    {
        // private const string BASE_URL = "https://api.jikan.moe/v4/top/anime";
        private readonly RestClient _client;
        private readonly (string, string?)[]? _defaultQueryParams;

        public Client(string baseUrl)
        {
            _client = new RestClient(baseUrl);
            _defaultQueryParams = null;
        }

        public Client(string baseUrl, (string, string?)[] defaultQueryParams)
        {
            _client = new RestClient(baseUrl);
            _defaultQueryParams = defaultQueryParams;
        }

        private void AddQueryParameters(RestRequest request, (string, string?)[]? query)
        {
            if (query != null)
                for (var i = 0; i < query.Length; i++)
                    request.AddParameter(query[i].Item1, query[i].Item2);
        }

        public async Task<string?> Get(string endpoint, (string, string?)[]? query)
        {
            var request = new RestRequest(endpoint);

            AddQueryParameters(request, _defaultQueryParams);
            AddQueryParameters(request, query);

            try
            {
                var response = await _client.ExecuteGetAsync(request);

                if (!response.IsSuccessful)
                {
                    Console.WriteLine(response.ErrorMessage);
                    throw new Exception(response.StatusDescription);
                }

                return response.Content;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
