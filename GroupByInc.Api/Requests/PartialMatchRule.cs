using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    public class PartialMatchRule
    {
        [JsonProperty("terms")] private int? _terms;

        [JsonProperty("termsGreaterThan")] private int? _termsGreaterThan;

        [JsonProperty("mustMatch")] private int? _mustMatch;

        [JsonProperty("percentage")] private bool? _percentage = false;

        public int? GetTerms()
        {
            return _terms;
        }

        public PartialMatchRule SetTerms(int? terms)
        {
            _terms = terms;
            return this;
        }

        public int? GetTermsGreaterThan()
        {
            return _termsGreaterThan;
        }

        public PartialMatchRule SetTermsGreaterThan(int? termsGreaterThan)
        {
            _termsGreaterThan = termsGreaterThan;
            return this;
        }

        public int? GetMustMatch()
        {
            return _mustMatch;
        }

        public PartialMatchRule SetMustMatch(int? mustMatch)
        {
            _mustMatch = mustMatch;
            return this;
        }

        public bool? GetPercentage()
        {
            return _percentage;
        }

        public PartialMatchRule SetPercentage(bool? percentage)
        {
            _percentage = percentage;
            return this;
        }

        public int? GetEffectiveGreaterThan()
        {
            if (_terms != null)
            {
                return _terms.Value - 1;
            }
            return _termsGreaterThan;
        }
    }
}