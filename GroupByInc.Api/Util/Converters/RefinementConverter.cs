using System;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Refinements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GroupByInc.Api.Util.Converters
{
    public class RefinementConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            //assume we can convert to anything for now
            return (objectType == typeof (Refinement));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            //explicitly specify the concrete type we want to create
            if (!string.IsNullOrEmpty(objectType.Name) && objectType.Name.Equals("Refinement"))
            {
                JObject jo = JObject.Load(reader);
                string type = Extensions.Value<string>(jo["type"]);
                switch (type)
                {
                    case "Value":
                        return jo.ToObject<RefinementValue>(serializer);
                    case "Range":
                        return jo.ToObject<RefinementRange>(serializer);
                }
            }

            return serializer.Deserialize(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //use the default serialization - it works fine
            serializer.Serialize(writer, value);
        }
    }
}