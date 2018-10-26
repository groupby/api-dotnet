using System;
using System.IO;
using System.Net;
using System.Text;
using GroupByInc.Api.Util;
using Newtonsoft.Json.Linq;
using Spring.Http;
using Spring.Http.Client;

namespace GroupByInc.Api
{
    public class CloudBridge
    {
        private static readonly string ExceptionFromBridge = "Exception from bridge: ";
        private static readonly string CloudHost = "groupbycloud.com";
        private readonly string _bridgeClusterUrl;
        private readonly string _bridgeRefinementsUrl;
        private readonly string _bridgeUrl;
        private readonly string _clientKey;
        private readonly IClientHttpRequestFactory _httpRequestFactory;
        private readonly Mappers _mappers;

        public CloudBridge(string clientKey, string customerId)
            : this(
                clientKey, $"https://{customerId}.{CloudHost}", new WebClientHttpRequestFactory(),
                new Mappers())
        {
        }

        public CloudBridge(string clientKey, string customerId, Mappers mappers)
            : this(
                clientKey, $"https://{customerId}.{CloudHost}", new WebClientHttpRequestFactory(),
                mappers)
        {
        }

        public CloudBridge(string clientKey, string baseUrl, IClientHttpRequestFactory httpRequestFactory)
            : this(clientKey, baseUrl, httpRequestFactory, new Mappers())
        {
        }

        public CloudBridge(string clientKey, string baseUrl, IClientHttpRequestFactory httpRequestFactory,
            Mappers mappers)
        {
            _clientKey = clientKey;
            _httpRequestFactory = httpRequestFactory;
            _mappers = mappers;

            var baseBridgeUrl = $"{baseUrl}:443/api/v1";

            _bridgeUrl = $"{baseBridgeUrl}/search";
            _bridgeRefinementsUrl = $"{baseBridgeUrl}/refinements";
            _bridgeClusterUrl = $"{baseBridgeUrl}/cluster";
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

        public JObject Map(IClientHttpResponse response, bool returnBinary)
        {
            return _mappers.ReadValue(response, returnBinary);
        }

        public JObject Search(Query query)
        {
            IClientHttpResponse response = FireRequest(_bridgeUrl, query.GetBridgeJson(_clientKey),
                query.IsReturnBinary());
            return Map(response, query.IsReturnBinary());
        }

        public JObject Refinements(Query query, string navigationName)
        {
            IClientHttpResponse response = FireRequest(_bridgeRefinementsUrl,
                query.GetBridgeRefinementsJson(_clientKey, navigationName),
                query.IsReturnBinary());
            return Map(response, query.IsReturnBinary());
        }

        private IClientHttpResponse FireRequest(string url, string body, bool returnsBinary)
        {
            //TODO : Implement retry mechanism
            IClientHttpResponse response = PostToBridge(url, body);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                HandleErrorStatus(response, returnsBinary);
            }
            return response;
        }

        private void HandleErrorStatus(IClientHttpResponse response, bool returnsBinary)
        {
            throw new IOException(ExceptionFromBridge + response.StatusCode + " " + response.StatusDescription + ", " +
                                  Map(response, returnsBinary)["errors"]);
        }

        private IClientHttpResponse PostToBridge(string url, string body)
        {
            Uri uri = new Uri(url);
            IClientHttpRequest request = _httpRequestFactory.CreateRequest(uri, HttpMethod.POST);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            request.Body = delegate(Stream stream) { stream.Write(bodyBytes, 0, bodyBytes.Length); };

            return request.Execute();
        }
    }
}