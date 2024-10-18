using RestSharp;

namespace ANN_Jikan.Services
{
    public class Client
    {
        private readonly RestClient _client;
        private readonly (string, string?)[]? _defaultQueryParams;

        public Client(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl);
            options.Timeout = TimeSpan.FromMilliseconds(2000);
            _client = new RestClient(options);
            _defaultQueryParams = null;
        }

        public Client(string baseUrl, (string, string?)[] defaultQueryParams)
        {
            var options = new RestClientOptions(baseUrl);
            options.Timeout = TimeSpan.FromMilliseconds(2000);
            _client = new RestClient(options);
            _defaultQueryParams = defaultQueryParams;
        }

        private void AddQueryParameters(RestRequest request, (string, string?)[]? query)
        {
            if (query != null)
                for (var i = 0; i < query.Length; i++)
                    request.AddParameter(query[i].Item1, query[i].Item2);
        }

        private async Task<string?> ExecRequest(RestRequest request)
        {
            try
            {
                var response = await _client.ExecuteGetAsync(request);

                if (!response.IsSuccessful)
                    throw new Exception(response.StatusDescription);

                return response.Content;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string?> Get()
        {
            var request = new RestRequest();
            AddQueryParameters(request, _defaultQueryParams);
            return await ExecRequest(request);
        }

        public async Task<string?> Get(string endpoint)
        {
            var request = new RestRequest(endpoint);
            AddQueryParameters(request, _defaultQueryParams);
            return await ExecRequest(request);
        }

        public async Task<string?> Get((string, string?)[] query)
        {
            var request = new RestRequest();
            AddQueryParameters(request, _defaultQueryParams);
            AddQueryParameters(request, query);
            return await ExecRequest(request);
        }

        public async Task<string?> Get(string endpoint, (string, string?)[] query)
        {
            var request = new RestRequest(endpoint);
            AddQueryParameters(request, _defaultQueryParams);
            AddQueryParameters(request, query);
            return await ExecRequest(request);
        }
    }
}
