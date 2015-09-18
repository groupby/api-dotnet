using System.Collections.Generic;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Refinements;
using GroupByInc.Api.Tags;
using GroupByInc.Api.Url;
using NUnit.Framework;

namespace GroupByInc.Api.Tests.Api.Tags
{
    [TestFixture]
    public class UrlFunctionsBugTest
    {
        [SetUp]
        public void SetUp()
        {
            UrlBeautifier.Injector.Set(new Dictionary<string, UrlBeautifier>());
            UrlBeautifier.CreateBeautifier(_defaultBeautifier);
            _urlBeautifier = UrlBeautifier.GetUrlBeautifiers()[_defaultBeautifier];
            _urlBeautifier.AddRefinementMapping('s', "size");
            _urlBeautifier.SetSearchMapping('q');
            _urlBeautifier.SetAppend("/index.html");
            _urlBeautifier.AddReplacementRule('/', ' ');
            _urlBeautifier.AddReplacementRule('\\', ' ');
        }

        private readonly string _defaultBeautifier = "default";
        private UrlBeautifier _urlBeautifier;

        [Test]
        public void RefinementAdditionWithMapping()
        {
            string refinementString = "Category Root~Athletics~Men's~Sneakers";
            string url = UrlFunctions.ToUrlAdd(_defaultBeautifier, "", new List<Navigation>(), "category_leaf_expanded",
                new RefinementValue().SetValue(refinementString).SetCount(5484));
            Assert.AreEqual(
                "/index.html?refinements=%7ecategory_leaf_expanded%3dCategory+Root%7eAthletics%7eMen%27s%7eSneakers",
                url);
        }
    }
}