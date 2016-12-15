using System;
using System.IO;
using GroupByInc.Api.Models.Refinements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Spring.Http;
using Spring.Rest.Client.Testing;
using System.Net;
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
            Assert.AreEqual(((JArray) results["availableNavigation"]).Count, 14);
            Assert.AreEqual(((JArray) results["records"]).Count, 50);
        }

        [Test]
        public void DeserializeRefinement()
        {
            object result =
                JsonConvert.DeserializeObject("{\"type\":\"Value\",\"count\":2144,\"value\":\"Category Root~Sale!\"}",
                    typeof(RefinementValue));
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetErrorFromBridge()
        {
            MockClientHttpRequest mockClientHttpRequest = new MockClientHttpRequest();
            HttpHeaders headers = new HttpHeaders();
            headers.Add("Content-Type", "application/json");

            string responseJson = Path.Combine(Environment.CurrentDirectory,
                string.Format(@"Resource{0}error.json", Path.DirectorySeparatorChar));
            mockClientHttpRequest.AndRespond(ResponseCreators.CreateWith(File.ReadAllText(responseJson), headers,
                HttpStatusCode.Unauthorized, "A bad thing happened"));

            MockClientHttpRequestFactory httpRequestFactory = new MockClientHttpRequestFactory();
            httpRequestFactory.AddMockClient(mockClientHttpRequest);
            Query query = new Query();
            query.SetReturnBinary(false);
            CloudBridge cloudBridge = new CloudBridge("****", "https://example.groupbycloud.com:443/api/v1",
                httpRequestFactory);

            try
            {
                JObject results = cloudBridge.Search(query);
                Assert.Fail("No exception thrown on bridge error");
            }
            catch (IOException e)
            {
                Assert.AreEqual(e.Message,
                    "Exception from bridge: Unauthorized A bad thing happened, This is the expected error");
            }
        }
    }
}