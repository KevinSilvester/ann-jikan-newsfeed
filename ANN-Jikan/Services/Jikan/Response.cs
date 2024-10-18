using System.Text.Json;

namespace ANN_Jikan.Services.Jikan
{
    #region Aliases
    using TExternalLinksResponse = List<ExternalLinksResponseData>;
    using TSearchResponse = List<SearchResponseData>;
    #endregion


    #region SearchResponse
    public static class JikanSearchRes
    {
        private static readonly IResponseParser<TSearchResponse> _parser =
            new SearchResponseParser();

        public static TSearchResponse Parse(string response) => _parser.Parse(response);
    }


    public class SearchResponseData
    {
        public int mal_id { get; set; }
        public double? score { get; set; }
        public string? title_english { get; set; }
        public required string title { get; set; }
    }

    class SearchResponse
    {
        public required List<SearchResponseData> data { get; set; }
    }

    class SearchResponseParser : IResponseParser<TSearchResponse>
    {
        public SearchResponseParser() { }

        public List<SearchResponseData> Parse(string response)
        {
            var searchResJson = JsonSerializer.Deserialize<SearchResponse>(response);
            if (searchResJson == null)
                throw new JsonException("Failed to deserialize SearchResponse");
            return searchResJson.data;
        }
    }
    #endregion


    #region ExternalLinksResponse
    public static class JikanExtenalLinksRes
    {
        private static readonly IResponseParser<TExternalLinksResponse> _parser =
            new ExternalLinksResponseParser();

        public static TExternalLinksResponse Parse(string response) => _parser.Parse(response);
    }

    public class ExternalLinksResponse
    {
        public required List<ExternalLinksResponseData> data { get; set; }
    }

    public class ExternalLinksResponseData
    {
        public required string url { get; set; }
        public required string name { get; set; }
    }

    class ExternalLinksResponseParser : IResponseParser<TExternalLinksResponse>
    {
        public ExternalLinksResponseParser() { }

        public TExternalLinksResponse Parse(string response)
        {
            var searchResJson = JsonSerializer.Deserialize<ExternalLinksResponse>(response);
            if (searchResJson == null)
                throw new JsonException("Failed to deserialize ExternalLinksResponse");
            return searchResJson.data;
        }
    }
    #endregion
}
