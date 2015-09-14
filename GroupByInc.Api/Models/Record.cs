using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    public class Record : AbstractRecord<Record>
    {
        [JsonProperty("refinementMatches")]
        private List<RefinementMatch> _refinementMatches;

        public List<RefinementMatch> GetRefinementMatches()
        {
            return _refinementMatches;
        }

        public Record SetRefinementMatches(List<RefinementMatch> refinementMatches)
        {
            _refinementMatches = refinementMatches;
            return this;
        }
    }
}
