using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    /// <summary>
    ///     Object that specifies sort field and direction
    /// </summary>
    public class Sort
    {
        /// <summary>
        /// </summary>
        public enum Order
        {
            Ascending,
            Descending
        }

        public static readonly Sort Relevance = new Sort().SetField("_relevance");

        [JsonProperty("order")] private Order _order = Order.Ascending;

        [JsonProperty("field")] private string _field;

        public string GetField()
        {
            return _field;
        }

        public Sort SetField(string field)
        {
            _field = field;
            return this;
        }

        public Order GetOrder()
        {
            return _order;
        }

        public Sort SetOrder(Order order)
        {
            _order = order;
            return this;
        }
    }
}