using System;
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
        public string WriteValueAsString<T>(T value)
        {
            return JsonConvert.SerializeObject(value, GetJsonSerializerSettings());
        }

        protected virtual JsonSerializerSettings GetJsonSerializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSerializerSettings.Converters.Add(new StringEnumConverter());
            jsonSerializerSettings.ContractResolver = new EmptyCollectionContractResolver();
            return jsonSerializerSettings;
        }

        protected virtual JsonSerializerSettings GetJsonDeserializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new RefinementConverter());
            return jsonSerializerSettings;
        }

        public virtual JObject ReadValue(IClientHttpResponse response, bool returnBinary)
        {
            //TODO handle binary format if returnBinary is true 

            // Read from the message stream
            using (StreamReader reader = new StreamReader(response.Body))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                return JToken.ReadFrom(jsonReader) as JObject;
            }
        }

        public virtual object ReadValue(JObject jObject, Type type)
        {
            // Read from the message stream
            JsonSerializer jsonSerializer = JsonSerializer.Create(GetJsonDeserializerSettings());
            return jsonSerializer.Deserialize(new JTokenReader(jObject), type);
        }

        public T CloneJson<T>(T source)
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