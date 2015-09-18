using GroupByInc.Api.Models;
using GroupByInc.Api.Requests;
using GroupByInc.Api.Util;
using Newtonsoft.Json.Linq;
using Spring.Http.Client;

namespace GroupByInc.Api
{
    public class CloudBridge : AbstractBridge
    {

        private static readonly string Dot = ".";
        private static readonly string CloudHost = "groupbycloud.com";
        private static readonly int CloudPort = 443;
        private static readonly string CloudPath = "/api/v1";
        private static readonly string UrlSuffix = Dot + CloudHost + Colon + CloudPort + CloudPath;

        public CloudBridge(string clientKey, string customerId)
            : this(clientKey, string.Format("{0}{1}{2}", Https, customerId, UrlSuffix), new WebClientHttpRequestFactory())
        {
        }

        public CloudBridge(string clientKey, string baseUrl, IClientHttpRequestFactory httpRequestFactory)
            : base(clientKey, baseUrl, httpRequestFactory)
        {
        }

        public CloudBridge(string clientKey, string baseUrl, IClientHttpRequestFactory httpRequestFactory, bool compressResponse)
            : base(clientKey, baseUrl, httpRequestFactory, compressResponse)
        {
        }

        public override JObject Map(IClientHttpResponse response, bool returnBinary)
        {
            return Mappers.ReadValue(response);
        }

        public override JObject MapRefinements(IClientHttpResponse response, bool returnBinary)
        {
            return Mappers.ReadValue(response);
        }
    }
}