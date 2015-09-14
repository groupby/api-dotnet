using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    public abstract class SelectedRefinement
    {
        public enum TypeEnum
        {
            Value,
            Range
        }

        [JsonProperty("_id")]
        private string _id;

        [JsonProperty("navigationName")]
        public string _navigationName;

        [JsonProperty("exclude")] //
        private bool? _exclude;

        public abstract TypeEnum Type { get; }

        public TypeEnum GetType()
        {
            return Type;
        }

        public string GetId()
        {
            return _id;
        }

        public SelectedRefinement SetId(string id)
        {
            _id = id;
            return this;
        }

        public string GetNavigationName()
        {
            return _navigationName;
        }

        public SelectedRefinement SetNavigationName(string navigationName)
        {
            _navigationName = navigationName;
            return this;
        }

        public bool IsRange()
        {
            return GetType() == TypeEnum.Range;
        }

        public abstract string ToTildeString();

        public bool? GetExclude()
        {
            return _exclude;;
        }

        public SelectedRefinement SetExclude(bool? exclude)
        {
            _exclude = exclude;
            return this;
        }
    }
}