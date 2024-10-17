using System.Text.Json;
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
        var responseJson = JsonSerializer.Deserialize<SearchResponse>(response);

        var expected = File.ReadAllText("../../../test-assets/jikan-search-naruto.json");
        var expectedJson = JsonSerializer.Deserialize<SearchResponse>(expected);

        Assert.NotNull(responseJson);
        Assert.NotNull(expectedJson);
        Assert.Equal(expectedJson.data.Length, responseJson.data.Length);
        Assert.Equal(expectedJson.data[0].mal_id, responseJson.data[0].mal_id);
    }

    [Fact]
    public async Task TestJikanExternal()
    {
        var response = await jikanApiClient.Get("20/external", null);
        Assert.NotNull(response);
        var responseJson = JsonSerializer.Deserialize<ExternalLinksResponse>(response);

        var expected = File.ReadAllText("../../../test-assets/jikan-external-naruto.json");
        var expectedJson = JsonSerializer.Deserialize<ExternalLinksResponse>(expected);

        Assert.NotNull(responseJson);
        Assert.NotNull(expectedJson);
        Assert.Equal(expectedJson.data.Length, responseJson.data.Length);
        Assert.Equal(expectedJson.data[0].url, responseJson.data[0].url);
    }
}
