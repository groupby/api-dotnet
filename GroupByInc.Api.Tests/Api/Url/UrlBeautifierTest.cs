using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using GroupByInc.Api.Exceptions;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Refinements;
using GroupByInc.Api.Url;
using GroupByInc.Api.Util;
using NUnit.Framework;

namespace GroupByInc.Api.Tests.Api.Url
{
    [TestFixture]
    public class UrlBeautifierTest
    {
        [SetUp]
        public void SetUp()
        {
//            _beautifier = new UrlBeautifier();
            UrlBeautifier.Injector.Set(new Dictionary<string, UrlBeautifier>());
            UrlBeautifier.CreateBeautifier("default");
            _beautifier = UrlBeautifier.GetUrlBeautifiers()["default"];
            _beautifier.ClearSavedFields();
        }

        private static UrlBeautifier _beautifier;

        public void AssertNavigation(string expectedNavigationName, string expectedValue, Navigation navigation)
        {
            Assert.AreEqual(expectedNavigationName, navigation.GetName());
            Assert.AreEqual(expectedValue, ((Refinement) navigation.GetRefinements()[0]).ToTildeString());
        }

        private void SetUpTestHeightCategoryAndSearch()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('t', "test");
            _beautifier.AddRefinementMapping('h', "height");
            _beautifier.AddRefinementMapping('c', "category");
        }

        private void SetUpTestHeightAndCategoryRefinements()
        {
            _beautifier.AddRefinementMapping('t', "test");
            _beautifier.AddRefinementMapping('h', "height");
            _beautifier.AddRefinementMapping('c', "category");
        }

        private static void ToAndFromUrl(string psearchString, string pRefinementString,
            params string[] pExpectedRefinementsValues)
        {
            string url = _beautifier.ToUrl(psearchString, pRefinementString);
            Query query =
                _beautifier.FromUrl(url);
            Assert.AreEqual(psearchString, query.GetQuery());

            OrderedDictionary orderedDictionary = query.GetNavigations();
            ICollection collection = orderedDictionary.Values;
            List<Navigation> navigations = CollectionUtils.CollectionToList<Navigation>(collection);
            for (int i = 0; i < pExpectedRefinementsValues.Length; i++)
            {
                string refinement = pExpectedRefinementsValues[i];
                Assert.AreEqual(refinement, ((RefinementValue) navigations[i].GetRefinements()[0]).GetValue());
            }
        }

        private static void TestFromUrl(string url, string searchString,
            params string[] pExpectedRefinementsValues)
        {
            Query query =
                _beautifier.FromUrl(url);
            if (!string.IsNullOrEmpty(searchString))
            {
                Assert.AreEqual(searchString, query.GetQuery());
            }

            OrderedDictionary orderedDictionary = query.GetNavigations();
            ICollection collection = orderedDictionary.Values;
            List<Navigation> navigations = CollectionUtils.CollectionToList<Navigation>(collection);
            for (int i = 0; i < pExpectedRefinementsValues.Length; i++)
            {
                string refinement = pExpectedRefinementsValues[i];
                Assert.AreEqual(refinement, ((RefinementValue) navigations[i].GetRefinements()[0]).GetValue());
            }
        }

        private void SetSearchAndIndex()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.SetAppend("/index.html");
        }

        [Test]
        public void TestAddSameRefinementMultipleTimes()
        {
            _beautifier.SetAppend(".html");
            SetUpTestHeightAndCategoryRefinements();
            string url = _beautifier.ToUrl(
                "", "test=value~test=value~test=value2~height=20in~category=computer accessories");
            Assert.AreEqual("/value/value2/20in/computer+accessories/tthc.html", url);
        }

        [Test]
        public void TestAppend()
        {
            _beautifier.SetAppend(".html");
            SetUpTestHeightAndCategoryRefinements();
            string url = _beautifier.ToUrl("", "test=value~height=20in~category=computer accessories");
            Assert.AreEqual("/value/20in/computer+accessories/thc.html", url);
        }

        [Test]
        public void TestAppendWithSlash()
        {
            _beautifier.SetAppend("/index.html");
            SetUpTestHeightAndCategoryRefinements();
            string url = _beautifier.ToUrl("", "test=value~height=20in~category=computer accessories");
            Assert.AreEqual("/value/20in/computer+accessories/thc/index.html", url);
        }

        [Test]
        public void TestCanonical()
        {
            SetUpTestHeightAndCategoryRefinements();
            Assert.AreEqual(
                _beautifier.ToUrl(
                    null, "~height=20in" +
                          "~category2=mice~cat3=wireless mice" + "~test=value~category=computer accessories"),
                _beautifier.ToUrl(
                    null, "~height=20in" + "~category=computer accessories~test=value" +
                          "~category2=mice~cat3=wireless mice"));
        }

        [Test]
        public void TestDeepDetailQuery()
        {
            Query query = _beautifier.FromUrl("http://example.com/details?p=4&id=243478931&b=test");
            Assert.AreEqual("~id=243478931", query.GetRefinementsString());
        }

        [Test]
        public void TestDeepSearchQuery()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('t', "test");
            Query query = _beautifier.FromUrl("http://example.com/path/to/search/this%20is%20a%20test/value/qt");

            Assert.AreEqual("this is a test", query.GetQuery());
            Assert.AreEqual("~test=value", query.GetRefinementsString());
        }

        [Test]
        public void TestDetailQuery()
        {
            Query query = _beautifier.FromUrl("http://example.com/details?id=243478931");
            Assert.AreEqual("~id=243478931", query.GetRefinementsString());
        }

        [Test]
        public void TestEmptyQueryString()
        {
            _beautifier.FromUrl("/value/20in/computer%20accessories/thc?");
            _beautifier.FromUrl("");
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException),
            ExpectedMessage = "This token: q is already mapped to: search")]
        public void TestExistingMapping()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('q', "quasorLightLevel");
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException))]
        public void TestFromUrlBadInsert()
        {
            SetSearchAndIndex();
            _beautifier.FromUrl("/black+decker/q/index.html?z=c2-B");
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException))]
        public void TestFromUrlBadInsert2()
        {
            SetSearchAndIndex();
            _beautifier.FromUrl("/black+decker/q/index.html?z=ii2-B");
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException))]
        public void TestFromUrlBadReplace()
        {
            SetSearchAndIndex();
            _beautifier.FromUrl("/black+decker/q/index.html?z=2-B--");
        }

        [Test]
        public void TestFromUrlEmptySuffix()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('d', "department");
            _beautifier.SetAppend("/index.html");
            Query query = _beautifier.FromUrl("http://example.com/aoeu/laptop/this+is+a+test/qd/");
            List<Navigation> navigations = CollectionUtils.CollectionToList<Navigation>(query.GetNavigations());
            Assert.AreEqual("this is a test", ((RefinementValue) navigations[0].GetRefinements()[0]).GetValue());
        }

        [Test]
        public void TestFromUrlInsertBadIndex()
        {
            SetSearchAndIndex();
            TestFromUrl("/black+decker/q/index.html?z=i26-R", "black decker");
        }

        [Test]
        public void TestFromUrlInsertBadIndex3()
        {
            SetSearchAndIndex();
            TestFromUrl("/black+decker/q/index.html?z=i0-R", "black decker");
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException))]
        public void TestFromUrlInsertMalformedIndex()
        {
            SetSearchAndIndex();
            _beautifier.FromUrl("/black+decker/q/index.html?z=i-1-R");
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException))]
        public void TestFromUrlInsertNoIndex()
        {
            SetSearchAndIndex();
            _beautifier.FromUrl("/black+decker/q/index.html?z=i-R");
        }

        [Test]
        public void TestFromUrlInsertValidEdgeIndex()
        {
            SetSearchAndIndex();
            TestFromUrl("/black+decker/q/index.html?z=i13-R-6-%26", "black&deckerR");
        }

        [Test]
        public void TestFromUrlReplaceBadIndex()
        {
            SetSearchAndIndex();
            TestFromUrl("/black+decker/q/index.html?z=26-R", "black decker");
        }

        [Test]
        public void TestFromUrlReplaceBadIndex3()
        {
            SetSearchAndIndex();
            TestFromUrl("/black+decker/q/index.html?z=0-R", "black decker");
        }

        [Test]
        public void TestFromUrlReplaceBadIndex4()
        {
            SetSearchAndIndex();
            TestFromUrl("/black+decker/q/index.html?z=13-R", "black decker");
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException))]
        public void TestFromUrlReplaceBadReplacementString()
        {
            SetSearchAndIndex();
            _beautifier.FromUrl("/black+decker/q/index.html?z=-1-R");
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException))]
        public void TestFromUrlReplaceNoIndex()
        {
            SetSearchAndIndex();
            _beautifier.FromUrl("/black+decker/q/index.html?z=-R");
        }

        [Test]
        public void TestFromUrlReplaceValidEdgeIndex()
        {
            SetSearchAndIndex();
            TestFromUrl("/black+decker/q/index.html?z=12-R", "black deckeR");
        }

        [Test]
        public void TestFromUrlWithMultipleReplace()
        {
            SetSearchAndIndex();
            _beautifier.AddReplacementRule('&', ' ');
            _beautifier.AddReplacementRule('B', 'b');
            _beautifier.AddReplacementRule('D', 'd');
            ToAndFromUrl("Black&Decker", null);
        }

        [Test]
        public void TestFromUrlWithOneInsert()
        {
            SetSearchAndIndex();
            TestFromUrl("/black+decker/q/index.html?z=i1-1", "1black decker");
        }

        [Test]
        public void TestFromUrlWithOneReplace()
        {
            SetSearchAndIndex();
            _beautifier.AddReplacementRule('&', ' ');
            ToAndFromUrl("black&decker", null);
        }

        [Test]
        public void TestFromUrlWithReplace()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('d', "department");
            _beautifier.AddRefinementMapping('c', "category");
            _beautifier.SetAppend("/index.html");
            TestFromUrl("/mice/wireless/dell/cdq/index.html?z=1-M-i14-123-18-D", "Dell", "Mice", "wireless123");
        }

        [Test]
        public void TestFromUrlWithReplaceAndInsertionsOrderMatters()
        {
            SetSearchAndIndex();
            _beautifier.AddReplacementRule('d', 'D');
            _beautifier.AddReplacementRule('1', null);
            _beautifier.AddReplacementRule('2', null);
            _beautifier.AddReplacementRule('3', null);
            _beautifier.AddReplacementRule('&', ' ');
            _beautifier.AddReplacementRule('b', 'B');
            string searchString = "123black&decker";
            string expected = "/Black+Decker/q";
            Assert.AreEqual(expected, _beautifier.ToUrl(searchString, null).Substring(0, expected.Length));
            ToAndFromUrl(searchString, null);
        }

        [Test]
        public void TestFromUrlWithReplaceFullUrl()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('d', "department");
            _beautifier.AddRefinementMapping('c', "category");
            _beautifier.SetAppend("/index.html");
            TestFromUrl(
                "www.example.com/mice/wireless/dell/cdq/index.html?z=1-M-i14-123-18-D", "Dell", "Mice", "wireless123");
        }

        [Test]
        public void TestFromUrlWithSlash()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('d', "department");
            _beautifier.SetAppend("/index.html");
            TestFromUrl("/taylor/PHOTO%252FCOMMODITIES/qd/index.html", null, "PHOTO/COMMODITIES");
        }

        [Test]
        public void TestFullSearchUrl()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('t', "test");
            string url = _beautifier.ToUrl("this is a test", "test=value");
            Assert.AreEqual("/this+is+a+test/value/qt", url);
        }

        [Test]
        public void TestInvalidReferenceBlock()
        {
            Query query = _beautifier.FromUrl(
                "http://example.com/this%20is%20a%20test/value/qtrs", null);

            Assert.AreEqual(null, query);
        }

        [Test]
//        [Ignore("Multiple beautifiers")]
        public void TestMultipleBeautifiers()
        {
            UrlBeautifier.CreateBeautifier("default2");
            UrlBeautifier urlBeautifier = UrlBeautifier.GetUrlBeautifiers()["default2"];
            _beautifier.AddRefinementMapping('t', "test");
            Assert.AreEqual("/value/t", _beautifier.ToUrl(null, "test=value"));
            Assert.AreEqual("?refinements=%7etest%3dvalue", urlBeautifier.ToUrl(null, "test=value"));
        }

        [Test]
        public void TestMultipleRefinements()
        {
            SetUpTestHeightAndCategoryRefinements();
            string url = _beautifier.ToUrl("", "test=value~height=20in~category=computer accessories");
            Assert.AreEqual("/value/20in/computer+accessories/thc", url);
        }

        [Test]
        public void TestQueryUrl()
        {
            _beautifier.SetSearchMapping('q');
            string url = _beautifier.ToUrl("this is a test", null);
            Assert.AreEqual("/this+is+a+test/q", url);
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException))]
        public void TestRange()
        {
            _beautifier.AddRefinementMapping('t', "test");
            Assert.AreEqual("/bob/t?refinements=%7eprice%3a10..20", _beautifier.ToUrl(null, "test=bob~price:10..20"));
            _beautifier.AddRefinementMapping('p', "price");
            Assert.AreEqual("/bob/t?refinements=%7eprice%3a10..20", _beautifier.ToUrl(null, "test=bob~price:10..20"));
        }

        [Test]
        public void TestRefinementsUrl()
        {
            _beautifier.AddRefinementMapping('t', "test");
            string url = _beautifier.ToUrl(null, "test=value");
            Assert.AreEqual("/value/t", url);
        }

        [Test]
        public void TestRefinementWithSlash()
        {
            _beautifier.AddRefinementMapping('t', "test");
            Assert.AreEqual("/photo%2fcommodity/t", _beautifier.ToUrl(null, "test=photo/commodity"));
        }

        [Test]
        public void TestSearchQuery()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('t', "test");
            Query query = _beautifier.FromUrl("http://example.com/this%20is%20a%20test/value/qt");
            Assert.AreEqual("this is a test", query.GetQuery());
            Assert.AreEqual("~test=value", query.GetRefinementsString());
        }

        [Test]
        public void TestSearchUrlBackAndForth()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('t', "test");
            string url = "/this%20is%20a%20test/value/qt";
            Query query = _beautifier.FromUrl(url);
            Assert.AreEqual("this is a test", query.GetQuery());

            List<Navigation> navigations = CollectionUtils.CollectionToList<Navigation>(query.GetNavigations());
            Assert.AreEqual("test", navigations[0].GetName());
            Assert.AreEqual("value", ((RefinementValue) navigations[0].GetRefinements()[0]).GetValue());
        }

        [Test]
        public void TestSearchWithSlash()
        {
            _beautifier.SetSearchMapping('q');
            Assert.AreEqual("/photo%2fcommodity/q", _beautifier.ToUrl("photo/commodity", null));
        }

        [Test]
        public void TestSimpleToUrlMultipleReplace()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddReplacementRule('/', '-');
            _beautifier.AddReplacementRule('T', 't');
            string searchString = "This is/a Test";
            Assert.AreEqual("/this+is-a+test/q?z=8-%2f-1-T-11-T", _beautifier.ToUrl(searchString, null));
            ToAndFromUrl(searchString, null);
        }

        [Test]
        public void TestSimpleToUrlMultipleReplaceOrderMatters()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddReplacementRule('a', null);
            _beautifier.AddReplacementRule('/', '-');
            _beautifier.AddReplacementRule('_', null);
            ToAndFromUrl("this _is/a _test", null);
        }

        [Test]
        public void TestSimpleToUrlMultipleReplaceWithEmpty()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddReplacementRule('/', null);
            _beautifier.AddReplacementRule('_', null);
            string searchString = "this _is/a _test";
            Assert.AreEqual("/this+isa+test/q?z=i9-%2f-i6-_-i10-_", _beautifier.ToUrl(searchString, null));
            ToAndFromUrl(searchString, null);
        }

        [Test]
        public void TestSimpleToUrlOneReplace()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddReplacementRule('/', '-');
            string searchString = "this is/a test";
            Assert.AreEqual("/this+is-a+test/q?z=8-%2f", _beautifier.ToUrl(searchString, null));
            ToAndFromUrl(searchString, null);
        }

        [Test]
        public void TestSimpleToUrlReplaceWithEmpty()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddReplacementRule('/', null);
            string searchString = "this is/a test";
            Assert.AreEqual("/this+isa+test/q?z=i8-%2f", _beautifier.ToUrl("this is/a test", null));
            ToAndFromUrl(searchString, null);
        }

        [Test]
        [ExpectedException(typeof (UrlBeautificationException),
            ExpectedMessage = "Vowels are not allowed to avoid Dictionary words appearing")]
        public void TestStopVowels()
        {
            _beautifier.AddRefinementMapping('u', "test");
            _beautifier.SetSearchMapping('e');
        }

        [Test]
        public void TestToAndFromUrlWithMultipleRefinementSpecificReplacements()
        {
            SetUpTestHeightCategoryAndSearch();
            _beautifier.SetAppend("/index.html");
            _beautifier.AddReplacementRule('&', ' ', UrlBeautifier.SearchNavigationName);
            _beautifier.AddReplacementRule('i', 'm', "height");
            _beautifier.AddReplacementRule('e', 'a', "category");
            string searchString = "test&query";
            string refinements = "test=val&ue~height=20-in~category=computer accessories";
            string url = _beautifier.ToUrl(searchString, refinements);
            string expected = "/test+query/val%26ue/20-mn/computar+accassorias";
            Assert.AreEqual(expected, url.Substring(0, expected.Length));
            ToAndFromUrl(searchString, refinements, "val&ue", "20-in", "computer accessories");
        }

        [Test]
        public void TestToAndFromUrlWithNullsearchString()
        {
            SetUpTestHeightCategoryAndSearch();
            _beautifier.SetAppend("/index.html");
            _beautifier.AddReplacementRule('e', 'e');
            string refinements = "test=~height=20-in~category=computer accessories";
            ToAndFromUrl(null, refinements, "", "20-in", "computer accessories");
        }

        [Test]
        public void TestToAndFromUrlWithRefinementSpecificReplacements()
        {
            SetUpTestHeightCategoryAndSearch();
            _beautifier.SetAppend("/index.html");
            _beautifier.AddReplacementRule('&', ' ', UrlBeautifier.SearchNavigationName);
            string searchString = "test&query";
            string refinements = "test=val&ue~height=20-in~category=computer accessories";
            string url = _beautifier.ToUrl(searchString, refinements);
            string expected = "/test+query/val%26ue";
            Assert.AreEqual(expected, url.Substring(0, expected.Length));
            ToAndFromUrl(searchString, refinements, "val&ue", "20-in", "computer accessories");
        }

        [Test]
        public void TestToAndFromUrlWithReplaceWithRegexSpecialChar()
        {
            SetUpTestHeightCategoryAndSearch();
            _beautifier.SetAppend("/index.html");
            _beautifier.AddReplacementRule('.', '%');
            string refinements = "test=val&ue~height=20-in~category=computer accessories";
            ToAndFromUrl("test&qu%ery", refinements, "val&ue", "20-in", "computer accessories");
        }

        [Test]
        public void TestToAndFromUrlWithReplaceWithRegexSpecialChar2()
        {
            SetUpTestHeightCategoryAndSearch();
            _beautifier.SetAppend("/index.html");
            _beautifier.AddReplacementRule('e', '.');
            string refinements = "test=val&ue~height=20-in~category=computer accessories";
            ToAndFromUrl("test&qu%ery", refinements, "val&ue", "20-in", "computer accessories");
        }

        [Test]
        public void TestToAndFromUrlWithReplaceWithSameChar()
        {
            SetUpTestHeightCategoryAndSearch();
            _beautifier.SetAppend("/index.html");
            _beautifier.AddReplacementRule('e', 'e');
            string refinements = "test=val&ue~height=20-in~category=computer accessories";
            ToAndFromUrl("test&qu%ery", refinements, "val&ue", "20-in", "computer accessories");
        }

        [Test]
        public void TestToAndFromUrlWithReplaceWithSpecialChar()
        {
            SetUpTestHeightCategoryAndSearch();
            _beautifier.SetAppend("/index.html");
            _beautifier.AddReplacementRule('e', '/');
            _beautifier.AddReplacementRule('a', '\\');
            string refinements = "test=val&ue~height=20-in~category=computer accessories";
            ToAndFromUrl("test&query", refinements, "val&ue", "20-in", "computer accessories");
        }

        [Test]
        public void TestToAndFromUrlWithReplaceWithSpecialChar2()
        {
            SetUpTestHeightCategoryAndSearch();
            _beautifier.SetAppend("/index.html");
            _beautifier.AddReplacementRule('e', '%');
            string refinements = "test=val&ue~height=20-in~category=computer accessories";
            ToAndFromUrl("test&qu%ery", refinements, "val&ue", "20-in", "computer accessories");
        }

        [Test]
        public void TestToUrlWithReplace()
        {
            SetUpTestHeightAndCategoryRefinements();
            _beautifier.SetSearchMapping('q');
            _beautifier.AddReplacementRule('/', '-');
            _beautifier.AddReplacementRule('&', null);
            string searchString = "test&query";
            string refinements = "test=value~height=20/in~category=computer accessories";
            string url = _beautifier.ToUrl(searchString, refinements);
            Assert.AreEqual("/value/20-in/computer+accessories/testquery/thcq?z=9-%2f-i38-%26", url);
            ToAndFromUrl(searchString, refinements, "value", "20/in", "computer accessories");
        }

        [Test]
        public void TestToUrlWithReplaceDash()
        {
            SetUpTestHeightAndCategoryRefinements();
            _beautifier.SetSearchMapping('q');
            _beautifier.AddReplacementRule('-', ' ');
            _beautifier.AddReplacementRule('&', null);
            string searchString = "test&query";
            string refinements = "test=value~height=20-in~category=computer accessories";
            string url = _beautifier.ToUrl(searchString, refinements);
            Assert.AreEqual("/value/20+in/computer+accessories/testquery/thcq?z=9---i38-%26", url);
            ToAndFromUrl(searchString, refinements, "value", "20-in", "computer accessories");
        }

        [Test]
        public void TestToUrlWithReplaceWithRefinement()
        {
            SetUpTestHeightCategoryAndSearch();
            _beautifier.AddReplacementRule('/', ' ');
            _beautifier.AddReplacementRule('&', ' ', UrlBeautifier.SearchNavigationName);
            string refinements = "test=val&ue~height=20/in~category=computer accessories";
            ToAndFromUrl("test&query", refinements, "val&ue", "20/in", "computer accessories");
        }

        [Test]
        public void testToUrlWithUnmappedRefinements()
        {
            _beautifier.AddRefinementMapping('h', "height");
            _beautifier.AddRefinementMapping('c', "category");
            _beautifier.SetSearchMapping('q');
            _beautifier.AddReplacementRule('-', ' ');
            _beautifier.AddReplacementRule('&', null);
            string searchString = "test&query";
            string refinements = "test=value~height=20-in~category=computer accessories";
            string url = _beautifier.ToUrl(searchString, refinements);
            Assert.AreEqual("/20+in/computer+accessories/testquery/hcq?z=3---i32-%26&refinements=%7etest%3dvalue", url);
            ToAndFromUrl(searchString, refinements, "20-in", "computer accessories", "value");
        }

        [Test]
        public void TestUnappend()
        {
            _beautifier.SetAppend(".html");
            _beautifier.AddRefinementMapping('t', "test");
            _beautifier.AddRefinementMapping('h', "height");
            Query query = _beautifier.FromUrl("/value/20in/th.html");

            List<Navigation> navigations = CollectionUtils.CollectionToList<Navigation>(query.GetNavigations());
            Assert.AreEqual(2, navigations.Count);
            AssertNavigation("test", "=value", navigations[0]);
            AssertNavigation("height", "=20in", navigations[1]);
        }

        [Test]
        public void TestUnappendWithSlash()
        {
            _beautifier.SetAppend("/index.html");
            _beautifier.AddRefinementMapping('t', "test");
            _beautifier.AddRefinementMapping('h', "height");
            Query query = _beautifier.FromUrl("/value/20in/th/index.html");

            List<Navigation> navigations = CollectionUtils.CollectionToList<Navigation>(query.GetNavigations());
            Assert.AreEqual(2, navigations.Count);
            AssertNavigation("test", "=value", navigations[0]);
            AssertNavigation("height", "=20in", navigations[1]);
        }

        [Test]
        public void TestUnencodePlus()
        {
            _beautifier.SetSearchMapping('q');
            _beautifier.AddRefinementMapping('d', "department");
            _beautifier.SetAppend("/index.html");
            TestFromUrl("/aoeu/laptop/MAGNOLIA+HOME+THEATR/qd/index.html", null, "MAGNOLIA HOME THEATR");
        }

        [Test]
        public void TestUnmappedFromUrl()
        {
            SetUpTestHeightAndCategoryRefinements();
            Query query = _beautifier.FromUrl(
                "/value/20in/computer%20accessories/thc?refinements=%7Ecategory2%3Dmice%7Ecat3%3Dwireless%20mice");

            List<Navigation> navigations = CollectionUtils.CollectionToList<Navigation>(query.GetNavigations());
            Assert.AreEqual(5, navigations.Count);
            AssertNavigation("test", "=value", navigations[0]);
            AssertNavigation("height", "=20in", navigations[1]);
            AssertNavigation("category", "=computer accessories", navigations[2]);
            AssertNavigation("category2", "=mice", navigations[3]);
            AssertNavigation("cat3", "=wireless mice", navigations[4]);
        }

        [Test]
        public void TestUnmappedToUrl()
        {
            SetUpTestHeightAndCategoryRefinements();
            string url = _beautifier.ToUrl(
                "", "test=value~height=20in~category2=mice~cat3=wireless mice~category=computer accessories");
            Assert.AreEqual(
                "/value/20in/computer+accessories/thc?refinements=%7ecategory2%3dmice%7ecat3%3dwireless+mice", url);
        }

        [Test]
        public void TestUnmappedToUrlWithModifiedName()
        {
            SetUpTestHeightAndCategoryRefinements();
            _beautifier.SetRefinementsQueryParameterName("r");
            string url = _beautifier.ToUrl(
                "", "test=value~height=20in~category2=mice~cat3=wireless mice~category=computer accessories");
            Assert.AreEqual(
                "/value/20in/computer+accessories/thc?r=%7ecategory2%3dmice%7ecat3%3dwireless+mice", url);
        }
    }
}