using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    public class RefinementMatch
    {
        public class Value
        {
            [JsonProperty("value")]
            private string _value;

            [JsonProperty("count")]
            private int? _count;

            public string GetValue()
            {
                return _value;
            }

            public Value SetValue(string value)
            {
                _value = value;
                return this;
            }

            public int? GetCount()
            {
                return _count;
            }

            public Value SetCount(int? count)
            {
                _count = count;
                return this;
            }
        }
        [JsonProperty("name")]
        private string _name;

        [JsonProperty("values")]
        private List<Value> _values;

        public string GetName()
        {
            return _name;
        }

        public RefinementMatch SetName(string name)
        {
            _name = name;
            return this;
        }

        public List<Value> GetValues()
        {
            return _values;
        }

        public RefinementMatch SetValues(List<Value> values)
        {
            _values = values;
            return this;
        }
    }
}
