using System.Text.Json;

namespace ANN_Jikan.ServiceProviders.Jikan
{
    #region SearchResponse
    public class SearchResponse
    {
        public required SearchResponse_Data[] data { get; set; }
    }

    public class SearchResponse_Data
    {
        public int mal_id { get; set; }
        public double? score { get; set; }
        public required string title_english { get; set; }
    }
    #endregion


    #region ExternalLinksResponse
    public class ExternalLinksResponse
    {
        public required ExternalLinksResponse_Data[] data { get; set; }
    }

    public class ExternalLinksResponse_Data
    {
        public required string url { get; set; }
        public required string name { get; set; }
    }
    #endregion
}
