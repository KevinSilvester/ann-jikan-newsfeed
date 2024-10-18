using System.Text.RegularExpressions;

namespace ANN_Jikan.ServiceProviders.Jikan
{
    public class JikanService
    {
        private readonly Client _apiClient;
        private readonly Regex _annIdRegex;

        public JikanService()
        {
            _apiClient = new Client(
                "https://api.jikan.moe/v4/anime",
                new (string, string?)[]
                {
                    ("sfw", null),
                    ("order_by", "popularity"),
                    ("type", "tv"),
                }
            );
            _annIdRegex = new Regex(@"id=(\d+)");
        }

        public async Task<List<SearchResponseData>> Search(string query)
        {
            var response = await _apiClient.Get("", new (string, string?)[] { ("q", query) });
            if (response == null)
                throw new Exception("ServiceError: Jikan Api call failed!");
            return JikanSearchRes.Parse(response);
        }

        public async Task<List<SearchResponseData>> GetPopularAiring()
        {
            var response = await _apiClient.Get(
                "",
                new (string, string?)[] { ("status", "airing") }
            );
            if (response == null)
                throw new Exception("ServiceError: Jikan Api call failed!");

            return JikanSearchRes.Parse(response);
        }

        public async Task<int?> GetANNId(int animeId)
        {
            var response = await _apiClient.Get($"{animeId}/external", null);
            if (response == null)
                throw new Exception("ServiceError: Failed to get external links!");

            var externalLinks = JikanExtenalLinksRes.Parse(response);
            if (externalLinks == null)
                return null;

            var ann = externalLinks.FirstOrDefault(link => link?.name == "ANN", null);
            if (ann == null)
                return null;

            var match = _annIdRegex.Match(ann.url);
            if (match.Success)
                return int.Parse(match.Groups[1].Value);
            return null;
        }
    }
}
