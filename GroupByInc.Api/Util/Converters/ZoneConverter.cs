using System;
using GroupByInc.Api.Models;
using GroupByInc.Api.Models.Zones;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GroupByInc.Api.Util.Converters
{
    public class ZoneConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            //assume we can convert to anything for now
            return (objectType == typeof(Zone));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //explicitly specify the concrete type we want to create
            if (!string.IsNullOrEmpty(objectType.Name) && objectType.Name.Equals("Zone"))
            {
                JObject jo = JObject.Load(reader);
                string type = Extensions.Value<string>(jo["type"]);
                switch (type)
                {
                    case "Content":
                        return jo.ToObject<ContentZone>(serializer);
                    case "Record":
                        return jo.ToObject<RecordZone<Record>>(serializer);
                    case "Banner":
                        return jo.ToObject<BannerZone>(serializer);
                    case "Rich_Content":
                        return jo.ToObject<RichContentZone>(serializer);
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
