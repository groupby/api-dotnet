using System.Collections.Generic;
using GroupByInc.Api.Util;

namespace GroupByInc.Api.Models
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

        public MatchStrategy SetRules(params PartialMatchRule [] rules)
        {
            CollectionUtils.AddAll(_rules, rules);
            return this;
        }
    }
}
