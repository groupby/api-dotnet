using System;
using Newtonsoft.Json;

namespace GroupByInc.Api.Models.Refinements
{
    public class RefinementValue : Refinement
    {

        [JsonProperty("value")]
        private string _value;

        public string GetValue()
        {
            return _value;
        }

        public RefinementValue SetValue(string value)
        {
            _value = value;
            return this;
        }

        [JsonProperty("type")]
        public override TypeEnum Type
        {
            get
            {
                return TypeEnum.Value;
            }
        }

        public RefinementValue SetCount(int count)
        {
            Count = count;
            return this;
        }

        public RefinementValue SetExclude(bool? exclude)
        {
            Exclude = exclude;
            return this;
        }

        public override String ToTildeString()
        {
            return "=" + _value;
        }
    }
}
