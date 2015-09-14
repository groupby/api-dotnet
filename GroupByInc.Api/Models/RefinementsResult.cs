using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    public class RefinementsResult
    {
        [JsonProperty("errors")]
        protected string Errors;

        [JsonProperty("navigation")]
        protected Navigation Navigation;

        public string GetErrors()
        {
            return Errors;
        }

        public Navigation GetNavigation()
        {
            return Navigation;
        }

        public RefinementsResult SetNavigation(Navigation navigation)
        {
            Navigation = navigation;
            return this;
        }
    }
}
