using System.Collections.Generic;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Refinements;
using GroupByInc.Api.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;

namespace GroupByInc.Api.Tests.Api.Models.Refinements
{
    [TestFixture]
    public class ResultsTest
    {
        [SetUp]
        public void Setup()
        {
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            _jsonSerializerSettings.Converters.Add(new StringEnumConverter());
        }

        private JsonSerializerSettings _jsonSerializerSettings;

        public void AssertNavigation(string expected, Navigation navigation)
        {
            Assert.AreEqual(expected,
                JsonConvert.SerializeObject(navigation, _jsonSerializerSettings));
        }

        [Test]
        public void CanSerializeValueRefinement()
        {
            RefinementValue refinementValue = new RefinementValue();
            refinementValue.SetValue("something");

            Assert.AreEqual("{\"value\":\"something\",\"count\":0,\"exclude\":false,\"type\":\"Value\"}",
                new Mappers().WriteValueAsString(refinementValue));
        }

        [Test]
        public void GetMultipleSelectedNavigationJson()
        {
            RefinementValue refinementValue = new RefinementValue();
            refinementValue.SetValue("Ö'=\"");

            RefinementRange refinementRange = new RefinementRange();
            refinementRange.SetLow("10").SetHigh("100");

            AssertNavigation("{\"name\":\"A\",\"range\":false,\"or\":false,\"sort\":\"Count_Ascending\"," +
                             "\"refinements\":[{\"low\":\"10\",\"high\":\"100\",\"count\":0," +
                             "\"exclude\":false,\"type\":\"Range\"},{\"value\":\"Ö'=\\\"\",\"count\":0," +
                             "\"exclude\":false,\"type\":\"Value\"}],\"metadata\":[]}",
                new Navigation().SetName("A")
                    .SetRefinements(new List<Refinement>(new Refinement[] { refinementRange, refinementValue })));


            AssertNavigation("{\"name\":\"A\",\"range\":false,\"or\":false,\"sort\":\"Count_Ascending\"," +
                             "\"refinements\":[{\"value\":\"Ö'=\\\"\",\"count\":0," +
                             "\"exclude\":false,\"type\":\"Value\"},{\"low\":\"10\",\"high\":\"100\",\"count\":0," +
                             "\"exclude\":false,\"type\":\"Range\"}],\"metadata\":[]}",
                new Navigation().SetName("A")
                    .SetRefinements(new List<Refinement>(new Refinement[] { refinementValue, refinementRange })));
        }

        [Test]
        public void GetSelectedNavigationJsonOneRange()
        {
            RefinementRange refinementRange = new RefinementRange();
            List<Refinement> refinements = new List<Refinement>();
            refinementRange.SetLow("10").SetHigh("100");
            refinements.Add(refinementRange);
            AssertNavigation("{\"name\":\"A\",\"range\":false,\"or\":false,\"sort\":\"Count_Ascending\"," +
                             "\"refinements\":[{\"low\":\"10\",\"high\":\"100\",\"count\":0," +
                             "\"exclude\":false,\"type\":\"Range\"}],\"metadata\":[]}",
                new Navigation().SetName("A").SetRefinements(refinements));
        }

        [Test]
        public void GetSelectedNavigationJsonOneValue()
        {
            RefinementValue refinementValue = new RefinementValue();
            List<Refinement> refinements = new List<Refinement>();
            refinementValue.SetValue("Ö'=\"");
            refinements.Add(refinementValue);
            AssertNavigation("{\"name\":\"A\",\"range\":false,\"or\":false,\"sort\":\"Count_Ascending\"," +
                             "\"refinements\":[{\"value\":\"Ö'=\\\"\",\"count\":0," +
                             "\"exclude\":false,\"type\":\"Value\"}],\"metadata\":[]}",
                new Navigation().SetName("A").SetRefinements(refinements));
        }
    }
}