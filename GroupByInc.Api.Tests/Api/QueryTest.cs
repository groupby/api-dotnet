using System;
using GroupByInc.Api.Models;
using NUnit.Framework;

namespace GroupByInc.Api.Tests.Api
{
    [TestFixture]
    internal class QueryTest
    {
        private Query _baseQuery;


        [SetUp]
        public void SetUp()
        {
            _baseQuery = new Query();
        }

        private void AssertQuery(String expected, Query actual)
        {
            Assert.AreEqual(expected, actual.GetBridgeJson("aoeu"));
        }


        [Test]
        public void NullSearch()
        {
            _baseQuery.SetPageSize(100);
            string expected = "{\"pruneRefinements\":true," +
                              "\"clientKey\":\"aoeu\"," +
                              "\"skip\":0,\"pageSize\":100,\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void TestSingleSort()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.SetCollection("docs");
            _baseQuery.SetArea("staging");
            _baseQuery.AddValueRefinement("redsox", "suck");
            _baseQuery.SetSort(new Sort().SetField("relevance").SetOrder(Sort.Order.Descending));

            string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"sort\":[{\"order\":\"Descending\",\"field\":\"relevance\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10," +
                "\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }


        [Test]
        public void TestSingleSortUsingRelevance()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.SetCollection("docs");
            _baseQuery.SetArea("staging");
            _baseQuery.AddValueRefinement("redsox", "suck");
            _baseQuery.SetSort(Sort.Relevance);

            string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"sort\":[{\"order\":\"Ascending\",\"field\":\"_relevance\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10," +
                "\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }


        [Test]
        public void TestMultipleSort()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.SetCollection("docs");
            _baseQuery.SetArea("staging");
            _baseQuery.AddValueRefinement("redsox", "suck");
            _baseQuery.SetSort(new Sort().SetField("relevance"),
                new Sort().SetField("brand").SetOrder(Sort.Order.Descending));

            string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"sort\":[{\"order\":\"Ascending\",\"field\":\"relevance\"},{\"order\":\"Descending\",\"field\":\"brand\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10," +
                "\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void TestSortScore()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.SetCollection("docs");
            _baseQuery.SetArea("staging");
            _baseQuery.AddValueRefinement("redsox", "suck");
            _baseQuery.SetSort(Sort.Relevance, new Sort().SetField("brand").SetOrder(Sort.Order.Descending));

            string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"sort\":[{\"order\":\"Ascending\",\"field\":\"_relevance\"},{\"order\":\"Descending\",\"field\":\"brand\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10," +
                "\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }
    }
}
