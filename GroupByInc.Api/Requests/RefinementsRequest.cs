using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    public class RefinementsRequest<R>
        where R : AbstractRequest<R>
    {
        [JsonProperty("originalQuery")]
        private R _originalQuery;

        [JsonProperty("navigationName")]
        private string _navigationName;

        public string GetNavigationName()
        {
            return _navigationName;
        }

        public RefinementsRequest<R> SetNavigationName(string navigationName)
        {
            _navigationName = navigationName;
            return this;
        }

        public R GetOriginalQuery()
        {
            return _originalQuery;
        }

        public RefinementsRequest<R> SetOriginalQuery(R originQuery)
        {
            _originalQuery = originQuery;
            return this;
        }
    }
}
