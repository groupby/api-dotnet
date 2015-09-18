using System.Collections.Generic;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Refinements;
using GroupByInc.Api.Tags;
using GroupByInc.Api.Url;
using NUnit.Framework;

namespace GroupByInc.Api.Tests.Api.Tags
{
    [TestFixture]
    public class UrlFunctionsTest
    {
        [SetUp]
        public void SetUp()
        {
            UrlBeautifier.Injector.Set(new Dictionary<string, UrlBeautifier>());
            UrlBeautifier.CreateBeautifier(_defaultBeautifier);
            _urlBeautifier = UrlBeautifier.GetUrlBeautifiers()[_defaultBeautifier];
            _urlBeautifier.SetSearchMapping('q');
            _urlBeautifier.SetAppend("/index.html");
        }

        private readonly string _defaultBeautifier = "default";
        private UrlBeautifier _urlBeautifier;

        [Test]
        public void TestNestedRefinementAdditionMapping()
        {
            _urlBeautifier.AddRefinementMapping('t', "product.title");
            string url = UrlFunctions.ToUrlAdd(_defaultBeautifier, "", new List<Navigation>(), "product.title",
                new RefinementValue().SetValue("Civil War").SetCount(87));

            Assert.AreEqual("/Civil+War/t/index.html", url);
        }

        [Test]
        public void TestNestedRefinementAdditionWithoutMapping()
        {
            string url = UrlFunctions.ToUrlAdd(_defaultBeautifier, "", new List<Navigation>(), "product.title",
                new RefinementValue().SetValue("Civil War").SetCount(87));
            Assert.AreEqual("/index.html?refinements=%7eproduct.title%3dCivil+War", url);
        }

        [Test]
        public void TestRefinementAdditionWithMapping()
        {
            _urlBeautifier.AddRefinementMapping('g', "gender");
            _urlBeautifier.AddRefinementMapping('t', "product");
            _urlBeautifier.AddRefinementMapping('s', "primarysport");
            _urlBeautifier.AddRefinementMapping('c', "simpleColorDesc");
            _urlBeautifier.AddRefinementMapping('l', "collections");
            _urlBeautifier.AddRefinementMapping('f', "league");

            List<Navigation> navigations = new List<Navigation>();
            navigations.Add(new Navigation().SetName("gender")
                .SetDisplayName("Gender")
                .SetRange(false)
                .SetRefinements(
                    new List<Refinement>(new Refinement[] {new RefinementValue().SetValue("Women")})
                ));

            navigations.Add(new Navigation().SetName("simpleColorDesc")
                .SetDisplayName("Color")
                .SetRange(false)
                .SetRefinements(
                    new List<Refinement>(new Refinement[] {new RefinementValue().SetValue("Pink")})
                ));

            string url = UrlFunctions.ToUrlAdd(_defaultBeautifier, "", navigations, "product",
                new RefinementValue().SetValue("Clothing").SetCount(87));

            Assert.AreEqual("/Women/Clothing/Pink/gtc/index.html", url);
        }

        [Test]
        public void TestRefinementAdditionWithMappingMultipleToUrl()
        {
            _urlBeautifier.AddRefinementMapping('g', "gender");
            _urlBeautifier.AddRefinementMapping('t', "product");
            _urlBeautifier.AddRefinementMapping('s', "primarysport");
            _urlBeautifier.AddRefinementMapping('c', "simpleColorDesc");
            _urlBeautifier.AddRefinementMapping('l', "collections");
            _urlBeautifier.AddRefinementMapping('f', "league");

            List<Navigation> navigations = new List<Navigation>();
            navigations.Add(new Navigation().SetName("gender")
                .SetDisplayName("Gender")
                .SetRange(false)
                .SetRefinements(
                    new List<Refinement>(new Refinement[] {new RefinementValue().SetValue("Women")})
                ));

            navigations.Add(new Navigation().SetName("simpleColorDesc")
                .SetDisplayName("Color")
                .SetRange(false)
                .SetRefinements(
                    new List<Refinement>(new Refinement[] {new RefinementValue().SetValue("Pink")})
                ));

            string url = UrlFunctions.ToUrlAdd(_defaultBeautifier, "", navigations, "gender",
                new RefinementValue().SetValue("Men").SetCount(87));

            Assert.AreEqual("/Women/Men/Pink/ggc/index.html", url);

            url = UrlFunctions.ToUrlAdd(_defaultBeautifier, "", navigations, "gender",
                new RefinementValue().SetValue("Kid").SetCount(87));

            Assert.AreEqual("/Women/Kid/Pink/ggc/index.html", url);
        }

        [Test]
        public void TestRefinementAdditionWithoutMapping()
        {
            List<Navigation> navigations = new List<Navigation>();
            navigations.Add(new Navigation().SetName("gender")
                .SetDisplayName("Gender")
                .SetRange(false)
                .SetRefinements(
                    new List<Refinement>(new Refinement[] {new RefinementValue().SetValue("Women")})
                ));

            navigations.Add(new Navigation().SetName("simpleColorDesc")
                .SetDisplayName("Color")
                .SetRange(false)
                .SetRefinements(
                    new List<Refinement>(new Refinement[] {new RefinementValue().SetValue("Pink")})
                ));

            string url = UrlFunctions.ToUrlAdd(
                _defaultBeautifier, "", navigations, "product", new RefinementValue().SetValue("Clothing").SetCount(
                    87));
            Assert.AreEqual("/index.html?refinements=%7egender%3dWomen%7esimpleColorDesc%3dPink%7eproduct%3dClothing",
                url);
        }

        [Test]
        public void TestRefinementAdditionWithoutMappingAndSpace()
        {
            List<Navigation> navigations = new List<Navigation>();
            navigations.Add(new Navigation().SetName("gender")
                .SetDisplayName("Gender")
                .SetRange(false)
                .SetRefinements(
                    new List<Refinement>(new Refinement[] {new RefinementValue().SetValue("Women")})
                ));

            navigations.Add(new Navigation().SetName("simpleColorDesc")
                .SetDisplayName("Color")
                .SetRange(false)
                .SetRefinements(
                    new List<Refinement>(new Refinement[] {new RefinementValue().SetValue("Pink")})
                ));

            string url = UrlFunctions.ToUrlAdd(
                _defaultBeautifier, "", navigations, "product", new RefinementValue().SetValue("Clothing Box").SetCount(
                    87));
            Assert.AreEqual(
                "/index.html?refinements=%7egender%3dWomen%7esimpleColorDesc%3dPink%7eproduct%3dClothing+Box", url);
        }
    }
}