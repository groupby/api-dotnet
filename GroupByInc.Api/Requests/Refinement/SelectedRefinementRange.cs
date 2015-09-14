using Newtonsoft.Json;

namespace GroupByInc.Api.Requests.Refinement
{
    public class SelectedRefinementRange : SelectedRefinement
    {
        [JsonProperty("low")]
        private string _low;

        [JsonProperty("high")]
        private string _high;

        public string GetLow()
        {
            return _low;
        }


        public SelectedRefinementRange SetLow(string low)
        {
            _low = low;
            return this;
        }

        public string GetHigh()
        {
            return _high;
        }

        public SelectedRefinementRange SetHigh(string high)
        {
            _high = high;
            return this;
        }

        [JsonProperty("type")]
        public override TypeEnum Type
        {
            get
            {
                return TypeEnum.Range;
            }
        }

        public override string ToTildeString()
        {
            return ":" + _low + ".." + _high;
        }
    }
}
