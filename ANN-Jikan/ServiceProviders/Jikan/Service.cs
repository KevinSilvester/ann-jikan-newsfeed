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
            var response1 = await _apiClient.Get(
                "",
                new (string, string?)[] { ("status", "airing"), ("page", "1") }
            );
            var response2 = await _apiClient.Get(
                "",
                new (string, string?)[] { ("status", "airing"), ("page", "2") }
            );

            if (response1 == null || response2 == null)
                throw new Exception("ServiceError: Jikan Api call failed!");

            var res1Parsed = JikanSearchRes.Parse(response1);
            var res2Parsed = JikanSearchRes.Parse(response2);
            return res1Parsed.Concat(res2Parsed).ToList();
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
