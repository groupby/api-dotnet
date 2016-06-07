using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    public class RefinementsRequest
    {
        [JsonProperty("originalQuery")]
        private Request _originalQuery;

        [JsonProperty("navigationName")]
        private string _navigationName;

        public string GetNavigationName()
        {
            return _navigationName;
        }

        public RefinementsRequest SetNavigationName(string navigationName)
        {
            _navigationName = navigationName;
            return this;
        }

        public Request GetOriginalQuery()
        {
            return _originalQuery;
        }

        public RefinementsRequest SetOriginalQuery(Request originQuery)
        {
            _originalQuery = originQuery;
            return this;
        }
    }
}