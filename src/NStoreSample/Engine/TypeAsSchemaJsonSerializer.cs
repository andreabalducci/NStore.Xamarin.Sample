using System;
using Newtonsoft.Json;
using NStore.Persistence.Sqlite;

namespace NStoreSample.Engine
{
    // !!! never use in production tied to clr type !!!
    public class TypeAsSchemaJsonSerializer : ISqlitePayloadSearializer
    {
        JsonSerializerSettings _settings;
        public TypeAsSchemaJsonSerializer()
        {
            _settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
        }

        public object Deserialize(string serialized)
        {
            return JsonConvert.DeserializeObject(serialized, _settings);
        }

        public string Serialize(object payload)
        {
            return JsonConvert.SerializeObject(payload, _settings);
        }
    }
}
