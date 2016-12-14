using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    public abstract class Refinement
    {
        public enum TypeEnum
        {
            Value,
            Range
        }

        [JsonProperty("_id")] //
        private string _id;

        [JsonProperty("count")] protected int Count;

        [JsonProperty("exclude")] protected bool? Exclude = false;

        public abstract TypeEnum Type { get; }
//
        public TypeEnum GetType()
        {
            return Type;
        }

        public int GetCount()
        {
            return Count;
        }

        public bool IsRange()
        {
            return false;
        }

        public bool? GetExclude()
        {
            return Exclude;
        }

        public abstract string ToTildeString();
    }
}