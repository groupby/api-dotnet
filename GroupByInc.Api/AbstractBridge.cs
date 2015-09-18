using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Spring.Http;
using Spring.Http.Client;

namespace GroupByInc.Api
{
    public abstract class AbstractBridge
    {
        public static readonly string Cluster = "/cluster";
        protected static readonly string Colon = ":";
        protected static readonly string Http = "http://";
        protected static readonly string Https = "https://";
        private static readonly string _search = "/search";
        private static readonly string RefinementsSearch = "/refinements";
        private static readonly string RefinementSearch = "/refinement";
        private static readonly string Body = "\nbody:\n";
        private static readonly string ExceptionFromBridge = "Exception from bridge: ";

        private readonly string _bridgeUrl;
        private readonly string _bridgeRefinementsUrl;
        private readonly string _bridgeRefinementSearchUrl;
        private readonly string _bridgeClusterUrl;
        private readonly string _clientKey;
        private IClientHttpRequestFactory _httpRequestFactory;
        private long _retryTimeout = 80;


        public AbstractBridge(string clientKey, string baseUrl, IClientHttpRequestFactory httpRequestFactory)
            : this(clientKey, baseUrl, httpRequestFactory, true)
        {
        }

        public AbstractBridge(string clientKey, string baseUrl, IClientHttpRequestFactory httpRequestFactory, bool compressResponse)
        {
            _clientKey = clientKey;
            _httpRequestFactory = httpRequestFactory;
            _bridgeUrl = baseUrl + _search;
            _bridgeRefinementsUrl = _bridgeUrl + RefinementsSearch;
            _bridgeRefinementSearchUrl = baseUrl + RefinementSearch;
            _bridgeClusterUrl = baseUrl + Cluster;
        }

        public string GetBridgeUrl()
        {
            return _bridgeUrl;
        }

        public string GetBridgeRefinementUrl()
        {
            return _bridgeRefinementsUrl;
        }

        public string GetClusterBridgeUrl()
        {
            return _bridgeClusterUrl;
        }

        public abstract JObject Map(IClientHttpResponse response, bool returnBinary);

        public abstract JObject MapRefinements(IClientHttpResponse response, bool returnBinary);

        public JObject Search(Query query)
        {
            IClientHttpResponse response = FireRequest(_bridgeUrl, query.GetBridgeJson(_clientKey), query.IsReturnBinary());
            return Map(response, query.IsReturnBinary());
        }

        public JObject Refinements(Query query, string navigationName)
        {
            IClientHttpResponse response = FireRequest(_bridgeRefinementsUrl, query.GetBridgeRefinementsJson(_clientKey, navigationName), query.IsReturnBinary());
            return MapRefinements(response, query.IsReturnBinary());
        }

        private IClientHttpResponse FireRequest(string url, string body, bool returnBinary)
        {

            //TODO : Implement loop mechanism
            IClientHttpResponse response = PostToBridge(url, body);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                HandleErrorStatus(response);
            }
            return response;
        }

        private void HandleErrorStatus(IClientHttpResponse response)
        {
            throw new IOException(ExceptionFromBridge + response.StatusCode + response.StatusDescription);
        }

        private IClientHttpResponse PostToBridge(string url, string body)
        {
            Uri uri = new Uri(url);
            IClientHttpRequest request = _httpRequestFactory.CreateRequest(uri, HttpMethod.POST);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            request.Body = delegate(Stream stream)
            {
                stream.Write(bodyBytes, 0, bodyBytes.Length);
            };

            return request.Execute();
        }
    }
}