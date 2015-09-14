using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    public class Template
    {

        [JsonProperty("_id")]
        private string _id;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("ruleName")]
        private string _ruleName;

        [JsonProperty("zones")]
        private Dictionary<string, Zone> _zones = new Dictionary<string, Zone>();

        public Template SetId(string value)
        {
            _id = value;
            return this;
        }

        public string GetId()
        {
            return _id;
        }

        public Template SetName(string value)
        {
            _name = value;
            return this;
        }

        public string GetName()
        {
            return _name;
        }

        public Template SetRuleName(string value)
        {
            _ruleName = value;
            return this;
        }

        public string GetRuleName()
        {
            return _ruleName;
        }

        public Template SetZones(Dictionary<string, Zone> value)
        {
            _zones = value;
            return this;
        }

        public Dictionary<string, Zone> GetZones()
        {
            return _zones;
        }
    }
}