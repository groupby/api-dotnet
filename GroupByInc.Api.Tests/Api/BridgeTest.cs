using System;
using System.IO;
using GroupByInc.Api.Http;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Refinements;
using GroupByInc.Api.Tests.Http.Client.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

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
            query.SetCollection("Variant").AddFields(new string[] {"*"});
            query.SetPageSize(50);
            query.SetReturnBinary(false);
            CloudBridge cloudBridge = new CloudBridge("****", "https://example.groupbycloud.com:443/api/v1",
                httpRequestFactory);
            Results results = cloudBridge.Search(query);
            Assert.AreEqual(results.GetArea(), "Production");
            Assert.AreEqual(results.GetAvailableNavigations().Count, 14);
            Assert.AreEqual(results.GetRecords().Count, 50);
        }

        [Test]
        public void DeserializeRefinement()
        {
            object result =
                JsonConvert.DeserializeObject("{\"type\":\"Value\",\"count\":2144,\"value\":\"Category Root~Sale!\"}",
                    typeof (RefinementValue));
            Assert.IsNotNull(result);
        }
    }
}