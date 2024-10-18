using ANN_Jikan.Services;
using ANN_Jikan.Services.ANN;

namespace ANN_Jikan.Tests;

public class ANNClientTests
{
    Client annApiClient;
    Client annNewsClient;

    public ANNClientTests()
    {
        annApiClient = new Client("https://cdn.animenewsnetwork.com/encyclopedia/api.xml");
        annNewsClient = new Client(
            "https://www.animenewsnetwork.com/news/2022-09-23/naruto-anime-leaves-netflix-in-october/.190054"
        );
    }

    [Fact]
    public async Task TestAnnApiGet()
    {
        var response = await annApiClient.Get(new (string, string?)[] { ("anime", "1825") });
        Assert.NotNull(response);
        var responseParsed = ANNNewsRes.Parse(response);

        var expected = File.ReadAllText("../../../test-assets/ann-naruto.xml");
        var expectedParsed = ANNNewsRes.Parse(expected);

        Assert.NotNull(responseParsed);
        Assert.NotNull(expectedParsed);
        Assert.Equal(expectedParsed.Count, responseParsed.Count);
        Assert.Equal(expectedParsed[0].url, responseParsed[0].url);
    }

    [Fact]
    public async Task TestAnnNewsGet()
    {
        var response = await annNewsClient.Get();
        Assert.NotNull(response);
        var responseParsed = ANNNewsArticlesRes.Parse(response);

        var expected = File.ReadAllText("../../../test-assets/ann-naruto-article-190054.html");
        var expectedParsed = ANNNewsArticlesRes.Parse(expected);

        Assert.NotNull(responseParsed);
        Assert.NotNull(expectedParsed);
        Assert.Equal(expectedParsed, responseParsed);
    } 
}
