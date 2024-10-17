using ANN_Jikan.ServiceProviders;
using ANN_Jikan.ServiceProviders.Jikan;

namespace ANN.Jikan.Tests;

public class JikanClientTests
{
    Client jikanApiClient;

    public JikanClientTests()
    {
        jikanApiClient = new Client(
            "https://api.jikan.moe/v4/anime",
            new (string, string?)[] { ("sfw", null), ("order_by", "popularity"), ("type", "tv") }
        );
    }

    [Fact]
    public async Task TestJikanSearch()
    {
        var response = await jikanApiClient.Get("", new (string, string?)[] { ("q", "naruto") });
        Assert.NotNull(response);
        var responseParsed = JikanSearchRes.Parse(response);

        var expected = File.ReadAllText("../../../test-assets/jikan-search-naruto.json");
        var expectedParrsed = JikanSearchRes.Parse(expected);

        Assert.NotNull(responseParsed);
        Assert.NotNull(expectedParrsed);
        Assert.Equal(expectedParrsed.Count, responseParsed.Count);
        Assert.Equal(expectedParrsed[0].mal_id, responseParsed[0].mal_id);
    }

    [Fact]
    public async Task TestJikanExternal()
    {
        var response = await jikanApiClient.Get("20/external", null);
        Assert.NotNull(response);
        var responseParsed = JikanExtenalLinksRes.Parse(response);

        var expected = File.ReadAllText("../../../test-assets/jikan-external-naruto.json");
        var expectedParsed = JikanExtenalLinksRes.Parse(expected);

        Assert.NotNull(responseParsed);
        Assert.NotNull(expectedParsed);
        Assert.Equal(expectedParsed.Count, responseParsed.Count);
        Assert.Equal(expectedParsed[0].url, responseParsed[0].url);
    }
}
