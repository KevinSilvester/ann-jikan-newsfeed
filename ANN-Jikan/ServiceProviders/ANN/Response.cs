using System.Xml.Serialization;

namespace ANN_Jikan.ServiceProviders.ANN
{
    [XmlRoot("ann")]
    public class ANNResponse
    {
        [XmlArray("anime")]
        [XmlArrayItem("news")]
        public required ANNResponse_News[] news { get; set; }
    }

    public class ANNResponse_News
    {
        [XmlAttribute("datetime")]
        public DateTime datetime { get; set; }

        [XmlAttribute("href")]
        public required string link { get; set; }

        [XmlText]
        public required string title { get; set; }
    }
}
