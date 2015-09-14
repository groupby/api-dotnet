using GroupByInc.Api.Http.Client;
using GroupByInc.Api.Models;
using GroupByInc.Api.Requests;
using GroupByInc.Api.Util;

namespace GroupByInc.Api
{
    public class Bridge : AbstractBridge<Request,Query,Record,Results>
    {

        public Bridge(string clientKey, string bridgeHost, int bridgePort)
            : this(clientKey, bridgeHost, bridgePort, false)
        {
        }

        private Bridge(string clientKey, string bridgeHost, int bridgePort, bool secure)
            : this(clientKey, (secure) ? Https : string.Format("{0}{1}{2}{3}", Http, bridgeHost, Colon, bridgePort), new WebClientHttpRequestFactory())
        {
        }

        public Bridge(string clientKey, string baseUrl, IClientHttpRequestFactory httpRequestFactory) : base(clientKey, baseUrl, httpRequestFactory)
        {
        }

        public Bridge(string clientKey, string baseUrl, IClientHttpRequestFactory httpRequestFactory, bool compressResponse) 
            : base(clientKey, baseUrl, httpRequestFactory, compressResponse)
        {
        }

        public override Results Map(IClientHttpResponse response, bool returnBinary)
        {
            return Mappers.ReadValue<Results>(response);
        }

        public override RefinementsResult MapRefinements(IClientHttpResponse response, bool returnBinary)
        {
            return Mappers.ReadValue<RefinementsResult>(response);
        }
    }
}
