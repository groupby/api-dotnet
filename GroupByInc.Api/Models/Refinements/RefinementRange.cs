using Newtonsoft.Json;

namespace GroupByInc.Api.Models.Refinements
{
    public class RefinementRange : Refinement
    {
        [JsonProperty("low")]
        private string _low;

        [JsonProperty("high")]
        private string _high;

        public string GetLow()
        {
            return _low;
        }


        public RefinementRange SetLow(string low)
        {
            _low = low;
            return this;
        }

        public string GetHigh()
        {
            return _high;
        }

        public RefinementRange SetHigh(string high)
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

        public RefinementRange SetCount(int count)
        {
            Count = count;
            return this;
        }

        public RefinementRange SetExclude(bool? exclude)
        {
            Exclude = exclude;
            return this;
        }

        public override string ToTildeString()
        {
            return ":" + _low + ".." + _high;
        }
    }
}
