using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    /// <summary>
    ///     RecordStart
    /// </summary>
    public class CustomUrlParam
    {
        [JsonProperty("key")]
        private string _key;

        [JsonProperty("value")]
        private string _value;

        public string GetKey()
        {
            return _key;
        }

        public CustomUrlParam SetKey(string key)
        {
            _key = key;
            return this;
        }


        public string GetValue()
        {
            return _value;
        }

        public CustomUrlParam SetValue(string value)
        {   
            _value = value;
            return this;
        }
    }
}