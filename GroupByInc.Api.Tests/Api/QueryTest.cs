using System.Collections.Generic;
using GroupByInc.Api.Requests;
using NUnit.Framework;
using MBias = GroupByInc.Api.Models.Bias;
using MBiasing = GroupByInc.Api.Models.Biasing;

namespace GroupByInc.Api.Tests.Api
{
    [TestFixture]
    internal class QueryTest
    {
        [SetUp]
        public void SetUp()
        {
            _query = new Query();
        }

        private Query _query;

        private void AssertQuery(string expected, Query actual)
        {
            Assert.AreEqual(expected, actual.GetBridgeJson("aoeu"));
        }

        private void AssertQuery(Query expected, Query actual)
        {
            AssertQuery(expected.GetBridgeJson("aoeu"), actual);
        }

        private void SetupGeneratedQuery()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.AddValueRefinement("redsox", "suck");
            _query.SetBiasingProfile("flange");
            _query.SetReturnBinary(false);
        }

        [Test]
        public void FilterString()
        {
            _query.AddValueRefinement("department", "VIDEO");
            _query.AddRangeRefinement("regularPrice", "200.000000", "400.000000");
            Assert.AreEqual("~department=VIDEO~regularPrice:200.000000..400.000000", _query.GetRefinementsString());
        }

        [Test]
        public void GenSubCollectionAndFrontEnd()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.AddValueRefinement("redsox", "suck");

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void GenWithLanguage()
        {
            SetupGeneratedQuery();
            _query.SetLanguage("lang_en");

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\",\"biasingProfile\":\"flange\",\"language\":\"lang_en\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";


            AssertQuery(expected, _query);
        }

        [Test]
        public void GenWithNoLanguage()
        {
            SetupGeneratedQuery();
            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\",\"biasingProfile\":\"flange\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void GenWithNullLanguage()
        {
            SetupGeneratedQuery();
            _query.SetLanguage(null);

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\",\"biasingProfile\":\"flange\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";

            AssertQuery(expected, _query);
        }

        [Test]
        public void IgnoreValueRefinements()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.AddValueRefinement("redsox", "suck", true);

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":true,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\"," +
                "\"collection\":\"docs\",\"area\":\"staging\",\"query\":\"boston\"," +
                "\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void MaxForRanges()
        {
            Query expected = new Query();
            expected.AddValueRefinement("department", "VIDEO");
            expected.AddRangeRefinement("regularPrice", "", "100.000000");

            _query.AddRefinementsByString("~department=VIDEO~regularPrice:..100.000000");
            AssertQuery(expected, _query);
        }

        [Test]
        public void MinForRanges()
        {
            Query expected = new Query();
            expected.AddValueRefinement("department", "VIDEO");
            expected.AddRangeRefinement("regularPrice", "200.000000", "");

            _query.AddRefinementsByString("~department=VIDEO~regularPrice:200.000000..");
            AssertQuery(expected, _query);
        }

        [Test]
        public void NullSearch()
        {
            _query.SetPageSize(100);
            const string expected = "{\"pruneRefinements\":true," +
                                    "\"clientKey\":\"aoeu\"," +
                                    "\"skip\":0,\"pageSize\":100,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }


        [Test]
        public void PruneRefinementsFalse()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.AddValueRefinement("redsox", "suck");
            _query.SetPruneRefinements(false);

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":false," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\"," +
                "\"area\":\"staging\",\"query\":\"boston\",\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void PruneRefinementsTrue()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.AddValueRefinement("redsox", "suck");
            _query.SetPruneRefinements(true);

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\"," +
                "\"area\":\"staging\",\"query\":\"boston\",\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";

            AssertQuery(expected, _query);
        }

        [Test]
        public void QuoteInRefinement()
        {
            _query.SetPageSize(100);
            _query.AddRefinementsByString("abc=ab'");
            const string expected = "{\"pruneRefinements\":true," +
                                    "\"refinements\":[{\"value\":\"ab'\",\"navigationName\":\"abc\",\"exclude\":false,\"type\":\"Value\"}]," +
                                    "\"clientKey\":\"aoeu\",\"skip\":0," +
                                    "\"pageSize\":100,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void RestrictedRefinements()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.AddValueRefinement("redsox", "suck");

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"clientKey\":\"aoeu\"," +
                "\"collection\":\"docs\",\"area\":\"staging\",\"query\":\"boston\"," +
                "\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void SplitTestCategory()
        {
            string[] split = _query.SplitRefinements("~category_leaf_expanded=Category Root~Athletics~Men's~Sneakers");
            Assert.AreEqual(new string[] {"category_leaf_expanded=Category Root~Athletics~Men's~Sneakers"}, split);
        }

        [Test]
        public void SplitTestCategoryLong()
        {
            const string reallyLongString =
                "~category_leaf_expanded=Category Root~Athletics~Men's~Sneakers~category_leaf_id=580003~" +
                "color=BLUE~color=YELLOW~color=GREY~feature=Lace Up~feature=Light Weight~brand=Nike";

            string[] split = _query.SplitRefinements(reallyLongString);
            Assert.AreEqual(new string[]
                {
                    "category_leaf_expanded=Category Root~Athletics~Men's~Sneakers", "category_leaf_id=580003",
                    "color=BLUE", "color=YELLOW", "color=GREY", "feature=Lace Up", "feature=Light Weight",
                    "brand=Nike"
                },
                split);
        }

        [Test]
        public void SplitTestMultipleCategory()
        {
            string[] split =
                _query.SplitRefinements(
                    "~category_leaf_expanded=Category Root~Athletics~Men's~Sneakers~category_leaf_id=580003");
            Assert.AreEqual(
                new string[]
                    {"category_leaf_expanded=Category Root~Athletics~Men's~Sneakers", "category_leaf_id=580003"}, split);
        }

        [Test]
        public void SplitTestNoCategory()
        {
            string[] split = _query.SplitRefinements("~gender=Women~simpleColorDesc=Pink~product=Clothing");
            Assert.AreEqual(new string[] {"gender=Women", "simpleColorDesc=Pink", "product=Clothing"}, split);
        }

        [Test]
        public void SplitTestRange()
        {
            string[] split = _query.SplitRefinements("test=bob~price:10..20");
            Assert.AreEqual(new string[] {"test=bob", "price:10..20"}, split);
        }

        [Test]
        public void StringSplitter()
        {
            _query.AddRefinementsByString("~department=VIDEO~regularPrice:200.000000..400.000000");
            Query expected = new Query();
            expected.AddValueRefinement("department", "VIDEO");
            expected.AddRangeRefinement("regularPrice", "200.000000", "400.000000");
            AssertQuery(expected, _query);
        }

        [Test]
        public void TestEmpty()
        {
            string[] split = _query.SplitRefinements("");
            Assert.AreEqual(new string[] {}, split);
        }

        [Test]
        public void TestMultipleSort()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.AddValueRefinement("redsox", "suck");
            _query.SetSort(new Sort().SetField("relevance"),
                new Sort().SetField("brand").SetOrder(Sort.Order.Descending));

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"sort\":[{\"order\":\"Ascending\",\"field\":\"relevance\"},{\"order\":\"Descending\",\"field\":\"brand\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10," +
                "\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void TestNull()
        {
            string[] split = _query.SplitRefinements(null);
            Assert.AreEqual(new string[] {}, split);
        }

        [Test]
        public void TestSingleSort()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.AddValueRefinement("redsox", "suck");
            _query.SetSort(new Sort().SetField("relevance").SetOrder(Sort.Order.Descending));

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"sort\":[{\"order\":\"Descending\",\"field\":\"relevance\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10," +
                "\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void TestSingleSortUsingRelevance()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.AddValueRefinement("redsox", "suck");
            _query.SetSort(Sort.Relevance);

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"sort\":[{\"order\":\"Ascending\",\"field\":\"_relevance\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10," +
                "\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void TestSortScore()
        {
            _query.SetQuery("boston");
            _query.AddCustomUrlParamsByString("fromGoogle=true&bigspender=1");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.AddValueRefinement("redsox", "suck");
            _query.SetSort(Sort.Relevance, new Sort().SetField("brand").SetOrder(Sort.Order.Descending));

            const string expected =
                "{\"customUrlParams\":[{\"key\":\"fromGoogle\",\"value\":\"true\"},{\"key\":\"bigspender\",\"value\":\"1\"}]," +
                "\"pruneRefinements\":true," +
                "\"refinements\":[{\"value\":\"suck\",\"navigationName\":\"redsox\",\"exclude\":false,\"type\":\"Value\"}]," +
                "\"sort\":[{\"order\":\"Ascending\",\"field\":\"_relevance\"},{\"order\":\"Descending\",\"field\":\"brand\"}]," +
                "\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"query\":\"boston\",\"skip\":0,\"pageSize\":10," +
                "\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void TestUtf8()
        {
            string[] split = _query.SplitRefinements("tëst=bäb~price:10..20");
            Assert.AreEqual(new string[] {"tëst=bäb", "price:10..20"}, split);
        }

        [Test]
        public void TestSessionAndVisitorId()
        {
            _query.SetQuery("boston");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.SetVisitorId("somevisitorhash");
            _query.SetSessionId("somesessionhash");

            const string expected =
                "{\"pruneRefinements\":true,\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"sessionId\":\"somesessionhash\",\"visitorId\":\"somevisitorhash\",\"query\":\"boston\",\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }


        [Test]
        public void TestQueryTimeBiasing()
        {
            _query.SetQuery("boston");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.SetVisitorId("somevisitorhash");
            _query.SetSessionId("somesessionhash");
            MBiasing biasing = new MBiasing();
            biasing.SetInfluence(12.1f);
            List<GroupByInc.Api.Models.Bias> biases = new List<GroupByInc.Api.Models.Bias>();
            biases.Add(new MBias().SetName("title").SetStrength(MBias.Strength.Absolute_Increase));
            biasing.SetBiases(biases);
            _query.SetBiasing(biasing);

            const string expected =
                "{\"pruneRefinements\":true,\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"sessionId\":\"somesessionhash\",\"visitorId\":\"somevisitorhash\",\"query\":\"boston\",\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false," +
                "\"biasing\":{\"biases\":[{\"name\":\"title\",\"strength\":\"Absolute_Increase\"}],\"influence\":12.1,\"augmentBiases\":false}}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void TestQueryWithBringToTop()
        {
            _query.SetQuery("boston");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.SetVisitorId("somevisitorhash");
            _query.SetSessionId("somesessionhash");
            MBiasing biasing = new MBiasing();
            List<string> bringToTop = new List<string>();
            bringToTop.Add("id1");
            biasing.SetBringToTop(bringToTop);
            _query.SetBiasing(biasing);

            const string expected =
                "{\"pruneRefinements\":true,\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"sessionId\":\"somesessionhash\",\"visitorId\":\"somevisitorhash\",\"query\":\"boston\",\"skip\":0," +
                "\"pageSize\":10,\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false," +
                "\"biasing\":{\"bringToTop\":[\"id1\"],\"augmentBiases\":false}}";
            AssertQuery(expected, _query);
        }

        [Test]
        public void TestQueryWithRestrictNavigation()
        {
            _query.SetQuery("boston");
            _query.SetCollection("docs");
            _query.SetArea("staging");
            _query.SetVisitorId("somevisitorhash");
            _query.SetSessionId("somesessionhash");
            _query.SetRestrictNavigation(new RestrictNavigation().SetName("brand").SetCount(2));

            const string expected =
                "{\"pruneRefinements\":true,\"clientKey\":\"aoeu\",\"collection\":\"docs\",\"area\":\"staging\"," +
                "\"sessionId\":\"somesessionhash\",\"visitorId\":\"somevisitorhash\",\"query\":\"boston\"," +
                "\"restrictNavigation\":{\"name\":\"brand\",\"count\":2},\"skip\":0,\"pageSize\":10," +
                "\"returnBinary\":false,\"disableAutocorrection\":true,\"wildcardSearchEnabled\":false}";
            AssertQuery(expected, _query);
        }
    }
}