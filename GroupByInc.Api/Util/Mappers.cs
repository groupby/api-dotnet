using System.IO;
using GroupByInc.Api.Http.Client;
using GroupByInc.Api.Models;
using GroupByInc.Api.Util.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GroupByInc.Api.Util
{
    public class Mappers
    {
        public static string WriteValueAsString<T>(T value)
        {
            return JsonConvert.SerializeObject(value, GetJsonSerializerSettings());
        }

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSerializerSettings.Converters.Add(new StringEnumConverter());
            jsonSerializerSettings.ContractResolver = new EmptyCollectionContractResolver();
            return jsonSerializerSettings;
        }

        private static JsonSerializerSettings GetJsonDeserializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new RefinementConverter());
            jsonSerializerSettings.Converters.Add(new DefaultConverter<AbstractRecord<Record>, Record>());
            jsonSerializerSettings.Converters.Add(new ZoneConverter());
            return jsonSerializerSettings;
        }

        public static T ReadValue<T>(IClientHttpResponse response)
        {
            // Read from the message stream
            using (StreamReader reader = new StreamReader(response.Body))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer jsonSerializer = JsonSerializer.Create(GetJsonDeserializerSettings());
                return jsonSerializer.Deserialize<T>(jsonReader);
            }
        }

        public static T CloneJson<T>(T source)
        {
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(
                JsonConvert.SerializeObject(source, GetJsonSerializerSettings()), GetJsonDeserializerSettings());
        }
    }
}