using System;
using System.IO;
using GroupByInc.Api.Models.Refinements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Spring.Http;
using Spring.IO;
using Spring.Rest.Client.Testing;
using MockClientHttpRequestFactory = GroupByInc.Api.Tests.Http.Client.Testing.MockClientHttpRequestFactory;

namespace GroupByInc.Api.Tests.Api
{
    [TestFixture]
    internal class BridgeTest
    {
        [Test]
        public void CallBridge()
        {
            MockClientHttpRequest mockClientHttpRequest = new MockClientHttpRequest();
            HttpHeaders headers = new HttpHeaders();
            headers.Add("Content-Type", "application/json");

            string fileToUpload = Path.Combine(Environment.CurrentDirectory,
                string.Format(@"Resource{0}result.json", Path.DirectorySeparatorChar));
            mockClientHttpRequest.AndRespond(ResponseCreators.CreateWith(File.ReadAllText(fileToUpload), headers));


            MockClientHttpRequestFactory httpRequestFactory = new MockClientHttpRequestFactory();
            httpRequestFactory.AddMockClient(mockClientHttpRequest);
            Query query = new Query();
            query.SetCollection("Variant").AddFields("*");
            query.SetPageSize(50);
            query.SetReturnBinary(false);
            CloudBridge cloudBridge = new CloudBridge("****", "https://example.groupbycloud.com:443/api/v1",
                httpRequestFactory);
            JObject results = cloudBridge.Search(query);
            Assert.AreEqual(results["area"].ToString(), "Production");
            Assert.AreEqual(((JArray) results["availableNavigation"]).Count, 15);
            Assert.AreEqual(((JArray) results["records"]).Count, 50);
        }

#if NET40
        [Test]
        public void CallBridgeCbor()
        {
            MockClientHttpRequest mockClientHttpRequest = new MockClientHttpRequest();
            HttpHeaders headers = new HttpHeaders();
            headers.Add("Content-Type", "application/cbor");

            string fileToUpload = Path.Combine(Environment.CurrentDirectory,
                string.Format(@"Resource{0}cbor_result.json", Path.DirectorySeparatorChar));
            byte[] readAllBytes = File.ReadAllBytes(fileToUpload);
            mockClientHttpRequest.AndRespond(ResponseCreators.CreateWith(new StreamResource(new MemoryStream(readAllBytes)), headers));


            MockClientHttpRequestFactory httpRequestFactory = new MockClientHttpRequestFactory();
            httpRequestFactory.AddMockClient(mockClientHttpRequest);
            Query query = new Query();
            query.SetCollection("Variant").AddFields("*");
            query.SetPageSize(50);
            query.SetReturnBinary(true);
            CloudBridge cloudBridge = new CloudBridge("****", "https://example.groupbycloud.com:443/api/v1",
                httpRequestFactory);
            JObject results = cloudBridge.Search(query);
            Assert.AreEqual(results["area"].ToString(), "Production");
            Assert.AreEqual(((JArray)results["availableNavigation"]).Count, 15);
            Assert.AreEqual(((JArray)results["records"]).Count, 50);
        }
#endif

        [Test]
        public void DeserializeRefinement()
        {
            object result =
                JsonConvert.DeserializeObject("{\"type\":\"Value\",\"count\":2144,\"value\":\"Category Root~Sale!\"}",
                    typeof(RefinementValue));
            Assert.IsNotNull(result);
        }
    }
}