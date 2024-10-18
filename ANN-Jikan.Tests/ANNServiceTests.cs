using ANN_Jikan.Services.ANN;

namespace ANN_Jikan.Tests;

public class ANNServiceTests
{
    ANNService annService;
    const string NEW_ARTICLE_LINK =
        "https://www.animenewsnetwork.com/news/2022-09-23/naruto-anime-leaves-netflix-in-october/.190054";

    public ANNServiceTests()
    {
        annService = new ANNService();
    }

    [Fact]
    public async Task TestGetNewsArticle()
    {
        var response = await annService.GetNewsArticle(NEW_ARTICLE_LINK);
        Assert.NotNull(response);
        
        var expected = File.ReadAllText("../../../test-assets/ann-naruto-article-190054.html");
        var expectedParsed = ANNNewsArticlesRes.Parse(expected);

        Assert.NotNull(expectedParsed);
        Assert.Equal(expectedParsed, response);
    }
}
