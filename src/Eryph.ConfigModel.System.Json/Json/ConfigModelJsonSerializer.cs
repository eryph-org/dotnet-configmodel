using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eryph.ConfigModel.Json
{
    public static class ConfigModelJsonSerializer
    {
        private static JsonSerializerOptions? _options;

        public static JsonSerializerOptions DefaultOptions
        {
            get
            {
                if (_options != null) return _options;

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                };
                options.Converters.Add(new ObjectAsPrimitiveConverter());
                options.Converters.Add(new JsonStringEnumConverter());
                _options = options;

                return _options;
            }
        }
        
        public static string Serialize<T>(T config, JsonSerializerOptions? options = default)
        {
            return JsonSerializer.Serialize(config, options ?? DefaultOptions);
        }

        public static IDictionary<object, object>? DeserializeToDictionary(string jsonString, JsonSerializerOptions? options = default)
        {

            return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString, options ?? DefaultOptions)?
                .ToDictionary(kv => (object)kv.Key, kv => kv.Value);
            
        }
        
        public static IDictionary<object, object>? DeserializeToDictionary(JsonElement element, JsonSerializerOptions? options = default)
        {
            return element.Deserialize<Dictionary<string, object>>(options ?? DefaultOptions)?
                .ToDictionary(kv => (object)kv.Key, kv => kv.Value);
            
        }
    }
}