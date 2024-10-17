using System.Xml.Linq;
using System.Xml.Serialization;

namespace ANN_Jikan.ServiceProviders.ANN
{
    using TNewsResponse = List<NewsResponseData>;

    public static class ANNNewsRes
    {
        private static readonly IResponseParser<TNewsResponse> _parser = new NewsResponseParser();

        public static TNewsResponse Parse(string response) => _parser.Parse(response);
    }

    public class NewsResponseData
    {
        public required string url { get; set; }
        public required string title { get; set; }
        public required string date { get; set; }
    }

    class NewsResponseParser : IResponseParser<TNewsResponse>
    {
        public TNewsResponse Parse(string response)
        {
            XDocument doc = XDocument.Parse(response);
            var newsArticles = doc.Descendants("news")
                .Select(news =>
                {
                    var urlAttr = news.Attribute("href");
                    var dateAttr = news.Attribute("datetime");

                    if (urlAttr == null || dateAttr == null)
                        throw new Exception("Failed to parse news article");

                    return new NewsResponseData
                    {
                        url = urlAttr.Value,
                        title = news.Value,
                        date = dateAttr.Value,
                    };
                })
                .ToList();
            return newsArticles;
        }
    }
}
