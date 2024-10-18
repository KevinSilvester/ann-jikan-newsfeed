namespace ANN_Jikan.ServiceProviders.ANN
{
    public class ANNService
    {
        private readonly Client _apiClient;
        private readonly Client _newsClient;

        public ANNService()
        {
            _apiClient = new Client("https://cdn.animenewsnetwork.com/encyclopedia/api.xml");
            _newsClient = new Client("");
        }

        public async Task<List<NewsResponseData>> GetNews(string animeId)
        {
            var response = await _apiClient.Get("", new (string, string?)[] { ("anime", animeId) });
            if (response == null)
                throw new Exception("ServiceError: ANN Api call failed!");

            return ANNNewsRes.Parse(response);
        }

        public async Task<string> GetNewsArticle(string url)
        {
            var response = await _apiClient.Get(url, null);
            if (response == null)
                throw new Exception("ServiceError: Failed to get news article!");

            return ANNNewsArticlesRes.Parse(response);
        }
    }
}
