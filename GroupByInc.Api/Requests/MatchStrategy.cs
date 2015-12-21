using System.Collections.Generic;

namespace GroupByInc.Api.Requests
{
    public class MatchStrategy
    {
        private List<PartialMatchRule> _rules =  new List<PartialMatchRule>();

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
