using System.IO;
using GroupByInc.Api.Util.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Spring.Http.Client;

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
            return jsonSerializerSettings;
        }

        public static JObject ReadValue(IClientHttpResponse response)
        {
            // Read from the message stream
            using (StreamReader reader = new StreamReader(response.Body))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                return JToken.ReadFrom(jsonReader) as JObject;
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