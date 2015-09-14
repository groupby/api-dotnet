using System;
using System.Collections.Generic;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Zones;
using GroupByInc.Api.Util;
using NUnit.Framework;

namespace GroupByInc.Api.Tests.Api.Models
{
    internal class ZoneTest
    {
        [Test]
        public void TestRecordZone()
        {
            try
            {
                List<RefinementMatch> refinementMatches = new List<RefinementMatch>();
                refinementMatches.Add(
                    new RefinementMatch().SetName("a").SetValues(
                        new List<RefinementMatch.Value>(
                            new RefinementMatch.Value[]
                            {
                                new RefinementMatch.Value().SetValue("c").SetCount(
                                    2),
                                new RefinementMatch.Value().SetValue("b").SetCount(
                                    1)
                            })));


                Record record =
                    new Record().SetId("abc")
                        .SetUrl("abc")
                        .SetTitle("abc")
                        .SetSnippet("abc")
                        .SetRefinementMatches(refinementMatches);


                Console.Write("record:     " + Mappers.WriteValueAsString(record));

                RecordZone<Record> zone =
                    new RecordZone<Record>().SetId("abc")
                        .SetName("abc")
                        .SetQuery("abc")
                        .SetRecords(new List<Record>(new Record[] {record}));
                Console.Write("zone:       " + Mappers.WriteValueAsString(zone));

                Dictionary<string, Zone> zones = new Dictionary<string, Zone>();
                zones.Add("abc", zone);
                Template template = new Template().SetName("abc").SetZones(zones);
                Console.Write("template:   " + Mappers.WriteValueAsString(template));

                Results results = new Results(); //.SetTemplate(template);
                Console.Write("results:    " + Mappers.WriteValueAsString(results));
            }
            catch
            {
                Assert.Fail("should be able to serialize");
            }
        }
    }
}