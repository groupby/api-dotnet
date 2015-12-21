using System;
using Newtonsoft.Json;

namespace GroupByInc.Api.Requests.Refinement
{
    public class SelectedRefinementValue : SelectedRefinement
    {
        [JsonProperty("value")]
        private string _value;

        public string GetValue()
        {
            return _value;
        }

        public SelectedRefinementValue SetValue(string value)
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

        public override String ToTildeString()
        {
            return "=" + _value;
        }
    }
}
