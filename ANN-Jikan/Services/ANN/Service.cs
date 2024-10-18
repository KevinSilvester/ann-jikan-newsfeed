namespace ANN_Jikan.Services.ANN
{
    public class ANNService
    {
        private readonly Client _apiClient;

        public ANNService()
        {
            _apiClient = new Client("https://cdn.animenewsnetwork.com/encyclopedia/api.xml");
        }

        public async Task<List<NewsResponseData>> GetNews(int animeId)
        {
            var response = await _apiClient.Get(
                new (string, string?)[] { ("anime", animeId.ToString()) }
            );
            if (response == null)
                throw new Exception("ServiceError: ANN Api call failed!");

            return ANNNewsRes.Parse(response);
        }

        public async Task<string> GetNewsArticle(string url)
        {
            Thread.Sleep(250);
            var newsClient = new Client(url);
            var response = await newsClient.Get();

            if (response == null)
                throw new Exception("ServiceError: Failed to get news article!");

            return ANNNewsArticlesRes.Parse(response);
        }
    }
}
