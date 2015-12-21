using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    /// <summary>
    ///     Metadata is associated with Navigation objects and Areas and allows the
    ///     merchandiser, from the command center to add additional information
    ///     about a navigation or area. For example there might be a UI hint that
    ///     the price range navigation should be displayed as a slider. Or you might
    ///     set an area metadata to inform the UI of the seasonal color scheme to
    ///     use.
    /// </summary>
    public class Metadata
    {
        [JsonProperty("key")]
        private string _key;

        [JsonProperty("value")]
        private string _value;

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}