using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;

namespace ann_jikan
{
    public class ANN
    {
        private static readonly RestClient ANN_jClient = new RestClient("https://cdn.animenewsnetwork.com/encyclopedia/api.xml");
    }
}
