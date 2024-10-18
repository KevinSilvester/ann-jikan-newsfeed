using System.Xml.Linq;
using HtmlAgilityPack;

namespace ANN_Jikan.ServiceProviders.ANN
{
    #region Aliases
    using TNewsResponse = List<NewsResponseData>;
    #endregion


    #region NewsResponse
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
                        url =
                            urlAttr.Value /* .Replace("https://www.animenewsnetwork.com", "")
                            .Replace(":", "") */
                        ,
                        title = news.Value,
                        date = dateAttr.Value,
                    };
                })
                .ToList();
            return newsArticles;
        }
    }
    #endregion


    #region NewsArticles
    public static class ANNNewsArticlesRes
    {
        private static readonly IResponseParser<string> _parser = new NewsArticlesParser();

        public static string Parse(string response) => _parser.Parse(response);
    }

    class NewsArticlesParser : IResponseParser<string>
    {
        private HtmlDocument _htmlDoc;

        public NewsArticlesParser()
        {
            _htmlDoc = new HtmlDocument();
        }

        public string Parse(string response)
        {
            _htmlDoc.LoadHtml(response);
            var articleElem = _htmlDoc.DocumentNode.QuerySelector(".KonaBody");
            return articleElem.InnerText.Trim();
        }
    }
    #endregion
}
