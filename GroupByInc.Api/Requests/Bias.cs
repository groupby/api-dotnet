using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    public class Bias
    {
        public enum Strength
        {
            Absolute_Increase,
            Strong_Increase,
            Medium_Increase,
            Weak_Increase,
            Leave_Unchanged,
            Weak_Decrease,
            Medium_Decrease,
            Strong_Decrease,
            Absolute_Decrease
        }

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("content")]
        private string _content;

        [JsonProperty("strength")]
        private Strength _strength;

        public string GetName()
        {
            return _name;
        }

        public Bias SetName(string name)
        {
            _name = name;
            return this;
        }

        public string GetContent()
        {
            return _content;
        }

        public Bias SetContent(string content)
        {
            _content = content;
            return this;
        }

        public Strength GetStrength()
        {
            return _strength;
        }

        public Bias SetStrength(Strength strength)
        {
            _strength = strength;
            return this;
        }
    }
}