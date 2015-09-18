using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GroupByInc.Api.Util.Converters
{
    public class DefaultConverter<TSObject> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            //assume we can convert to anything for now
            return (objectType == typeof (TSObject));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            //explicitly specify the concrete type we want to create
            JObject jo = JObject.Load(reader);
            return jo.ToObject<TSObject>(serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //use the default serialization - it works fine
            serializer.Serialize(writer, value);
        }
    }
}