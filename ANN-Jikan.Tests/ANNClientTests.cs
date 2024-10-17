using System.Text.Json;
using System.Xml.Serialization;
using ANN_Jikan.ServiceProviders;
using ANN_Jikan.ServiceProviders.ANN;

namespace ANN.Jikan.Tests;

public class ANNClientTests
{
    Client annApiClient;

    public ANNClientTests()
    {
        annApiClient = new Client("https://cdn.animenewsnetwork.com/encyclopedia/api.xml");
    }

    [Fact]
    public async Task TestJikanSearch()
    {
        var response = await annApiClient.Get("", new (string, string?)[] { ("anime", "1825") });
        Assert.NotNull(response);
        var responseParsed = ANNNewsRes.Parse(response);

        var expected = File.ReadAllText("../../../test-assets/ann-naruto.xml");
        var expectedParsed = ANNNewsRes.Parse(expected);

        Assert.NotNull(responseParsed);
        Assert.NotNull(expectedParsed);
        Assert.Equal(expectedParsed.Count, responseParsed.Count);
        Assert.Equal(expectedParsed[0].url, responseParsed[0].url);
    }
}
