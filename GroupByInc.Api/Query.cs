using System.Collections.Generic;
using GroupByInc.Api.Requests;
using GroupByInc.Api.Util;
using Newtonsoft.Json;
using MatchStrategy = GroupByInc.Api.Models.MatchStrategy;
using PartialMatchRule = GroupByInc.Api.Requests.PartialMatchRule;
using Sort = GroupByInc.Api.Models.Sort;

namespace GroupByInc.Api
{
    public class Query : AbstractQuery<Request, Query>
    {
        [JsonProperty("sort")]
        List<Sort> _sort = new List<Sort>();

        [JsonProperty("matchStrategy")]
        private MatchStrategy _matchStrategy;

        [JsonProperty("wildcardSearchEnabled")]
        private bool _wildcardSearchEnabled;

        protected static Requests.MatchStrategy ConvertPartialMatchStrategy(MatchStrategy strategy)
        {
            Requests.MatchStrategy convertedStrategy = null;
            if (strategy != null)
            {
                if (!CollectionUtils.IsNullOrEmpty(strategy.GetRules()))
                {
                    convertedStrategy = new Requests.MatchStrategy();
                    foreach (Models.PartialMatchRule rule in strategy.GetRules())
                    {
                        convertedStrategy.GetRules().Add(ConvertPartialMatchRule(rule));
                    }
                }
            }
            return convertedStrategy;
        }

        private static PartialMatchRule ConvertPartialMatchRule(Models.PartialMatchRule rule)
        {
            if (rule != null)
            {
                return new PartialMatchRule().SetTerms(rule.GetTerms())
                    .SetTermsGreaterThan(rule.GetTermsGreaterThan())
                    .SetMustMatch(rule.GetMustMatch())
                    .SetPercentage(rule.GetPercentage());
            }
            return null;
        }

        protected override Request GenerateRequest()
        {
            Request request = new Request();
            if (!CollectionUtils.IsNullOrEmpty(_sort))
            {
                foreach (Sort s in _sort)
                {
                    request.SetSort(ConvertSort(s));
                }
            }
            return request;
        }

        protected override RefinementsRequest<Request> PopulateRefinementRequest()
        {
            return new RefinementsRequest<Request>().SetOriginalQuery(GenerateRequest());
        }

        public Query SetSort(params Sort[] sort)
        {
            CollectionUtils.AddAll(_sort, sort);
            return this;
        }

        public bool IsWildcardSearchEnabled()
        {
            return _wildcardSearchEnabled;
        }

        public Query SetWildcardSearchEnabled(bool wildcardSearchEnabled)
        {
            this._wildcardSearchEnabled = wildcardSearchEnabled;
            return this;
        }

        public List<Sort> GetSort()
        {
            return _sort;
        }

        public Query SetMatchStrategy(MatchStrategy matchStrategy)
        {
            _matchStrategy = matchStrategy;
            return this;
        }
    }
}
