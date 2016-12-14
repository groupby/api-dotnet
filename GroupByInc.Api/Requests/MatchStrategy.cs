using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    public class MatchStrategy
    {
        [JsonProperty("rules")] private List<PartialMatchRule> _rules = new List<PartialMatchRule>();

        public List<PartialMatchRule> GetRules()
        {
            return _rules;
        }

        public MatchStrategy SetRules(List<PartialMatchRule> rules)
        {
            _rules = rules;
            return this;
        }
    }
}