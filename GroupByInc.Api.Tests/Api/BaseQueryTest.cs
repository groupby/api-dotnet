using System;
using NUnit.Framework;

namespace GroupByInc.Api.Tests.Api
{
    [TestFixture]
    class BaseQueryTest
    {
        private BaseQuery _baseQuery = new BaseQuery();

        private void AssertQuery(BaseQuery expected, BaseQuery actual)
        {
            AssertQuery(expected.GetBridgeJson("aoeu"), actual);
        }

        private void AssertQuery(String expected, BaseQuery actual)
        {
            Assert.AreEqual(expected, actual.GetBridgeJson("aoeu"));
        }

        [SetUp]
        public void SetUp()
        {
            _baseQuery = new BaseQuery();
        }

        [Test]
        public void FilterString()
        {
            _baseQuery.AddValueRefinement("department", "VIDEO");
            _baseQuery.AddRangeRefinement("regularPrice", "200.000000", "400.000000");
            Assert.AreEqual("~department=VIDEO~regularPrice:200.000000..400.000000", _baseQuery.GetRefinementsString());
        }

        [Test]
        public void StringSplitter()
        {
            _baseQuery.AddRefinementsByString("~department=VIDEO~regularPrice:200.000000..400.000000");
            BaseQuery expected = new BaseQuery();
            expected.AddValueRefinement("department", "VIDEO");
            expected.AddRangeRefinement("regularPrice", "200.000000", "400.000000");
            AssertQuery(expected, _baseQuery);
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
        public void QuoteInRefinement()
        {
            _baseQuery.SetPageSize(100);
            _baseQuery.AddRefinementsByString("abc=ab'");
            string expected = "{\"pruneRefinements\":true," +
                              "\"refinements\":[{\"value\":\"ab'\",\"navigationName\":\"abc\",\"exclude\":false,\"type\":\"Value\"}]," +
                              "\"clientKey\":\"aoeu\",\"skip\":0," +
                              "\"pageSize\":100,\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void MinForRanges()
        {
            BaseQuery expected = new BaseQuery();
            expected.AddValueRefinement("department", "VIDEO");
            expected.AddRangeRefinement("regularPrice", "200.000000", "");

            _baseQuery.AddRefinementsByString("~department=VIDEO~regularPrice:200.000000..");
            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void MaxForRanges()
        {
            BaseQuery expected = new BaseQuery();
            expected.AddValueRefinement("department", "VIDEO");
            expected.AddRangeRefinement("regularPrice", "", "100.000000");

            _baseQuery.AddRefinementsByString("~department=VIDEO~regularPrice:..100.000000");
            AssertQuery(expected, _baseQuery);
        }

        private void SetupGeneratedQuery()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.AddValueRefinement("redsox", "suck");
            _baseQuery.SetBiasingProfile("flange");
            _baseQuery.SetReturnBinary(false);
        }

        [Test]
        public void GenWithNoLanguage()
        {
            SetupGeneratedQuery();
            string expected = "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                              "\"pruneRefinements\":true," +
                              "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                              "\"clientKey\":\"aoeu\",\"biasingProfile\":\"flange\"," +
                              "\"query\":\"boston\",\"skip\":0,\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void GenWithNullLanguage()
        {
            SetupGeneratedQuery();
            _baseQuery.SetLanguage(null);

            string expected = "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                              "\"pruneRefinements\":true," +
                              "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                              "\"clientKey\":\"aoeu\",\"biasingProfile\":\"flange\"," +
                              "\"query\":\"boston\",\"skip\":0,\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true}";

            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void GenWithLanguage()
        {
            SetupGeneratedQuery();
            _baseQuery.SetLanguage("lang_en");

            string expected = "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                              "\"pruneRefinements\":true," +
                              "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                              "\"clientKey\":\"aoeu\",\"biasingProfile\":\"flange\",\"language\":\"lang_en\"," +
                              "\"query\":\"boston\",\"skip\":0,\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true}";


            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void GenSubCollectionAndFrontEnd()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.SetCollection("docs");
            _baseQuery.SetArea("staging");
            _baseQuery.AddValueRefinement("redsox", "suck");

            string expected = "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                              "\"pruneRefinements\":true," +
                              "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                              "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                              "\"query\":\"boston\",\"skip\":0,\"pageSize\":10,\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void PruneRefinementsFalse()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.SetCollection("docs");
            _baseQuery.SetArea("staging");
            _baseQuery.AddValueRefinement("redsox", "suck");
            _baseQuery.SetPruneRefinements(false);

            string expected = "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":false," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\"," +
                "\"area\":\"staging\",\"query\":\"boston\",\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void PruneRefinementsTrue()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.SetCollection("docs");
            _baseQuery.SetArea("staging");
            _baseQuery.AddValueRefinement("redsox", "suck");
            _baseQuery.SetPruneRefinements(true);

            string expected = "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\"," +
                "\"area\":\"staging\",\"query\":\"boston\",\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":true,\"disableAutocorrection\":true}";

            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void RestrictedRefinements()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.SetCollection("docs");
            _baseQuery.SetArea("staging");
            _baseQuery.AddValueRefinement("redsox", "suck");

            string expected = "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\"," +
                "\"collection\":\"docs\",\"area\":\"staging\",\"query\":\"boston\"," +
                "\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }

        [Test]
        public void IgnoreValueRefinements()
        {
            _baseQuery.SetQuery("boston");
            _baseQuery.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _baseQuery.SetCollection("docs");
            _baseQuery.SetArea("staging");
            _baseQuery.AddValueRefinement("redsox", "suck", true);

            string expected = "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":true,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\"," +
                "\"collection\":\"docs\",\"area\":\"staging\",\"query\":\"boston\"," +
                "\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":true,\"disableAutocorrection\":true}";
            AssertQuery(expected, _baseQuery);
        }
    }
}
