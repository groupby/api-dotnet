using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    public abstract class Zone
    {

        public enum Type
        {
            Content,
            Record,
            Banner,
            Rich_Content
        }

        [JsonProperty("_id")] //
        protected string Id;

        [JsonProperty("name")] //
        protected string Name;
        
        public abstract Type GeType();


        public string GetId()
        {
            return Id;
        }
        
        public string GetName()
        {
            return Name;
        }
    }
}