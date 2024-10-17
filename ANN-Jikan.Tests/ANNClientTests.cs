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
        var response = await annApiClient.Get("", new (string, string?)[] { ("id", "1825") });
        Assert.NotNull(response);
        XmlSerializer serializer = new(typeof(ANNResponse));
        var responseXml = serializer.Deserialize(new StringReader(response)) as ANNResponse;

        var expected = File.ReadAllText("../../../test-assets/ann-naruto.xml");
        var expectedXml = serializer.Deserialize(new StringReader(expected)) as ANNResponse;

        var responseJson = JsonSerializer.Serialize(
            responseXml,
            new JsonSerializerOptions { WriteIndented = true }
        );

        Assert.NotNull(responseXml);
        Assert.NotNull(expectedXml);
        Console.WriteLine(responseJson);
        Assert.Equal(expectedXml.news.Length, responseXml.news.Length);
        // Assert.Equal(expectedXml.news.Length, responseXml.news.Length);
        // Assert.Equal(expectedXml.news[0].link, responseXml.news[0].link);
    }
}
