using System.Collections.Generic;
using System.Text.Json;

namespace Eryph.ConfigModel.Json
{
    public static class ConfigModelJsonSerializer
    {
        public static string Serialize<T>(T config)
        {
            return JsonSerializer.Serialize(config,
                new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        }

        public static IDictionary<string, object> DeserializeToDictionary(string jsonString)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ObjectAsPrimitiveConverter());
            return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString, options);

        }
    }
}