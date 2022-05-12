using System.Collections.Generic;
using System.Text.Json;

namespace Eryph.ConfigModel.Json
{
    /// <summary>
    /// Helper class to convert a json string to a dictionary. 
    /// </summary>
    public static class JsonToDictionary
    {
        /// <summary>
        /// Deserialize json string a Dictionary.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static IDictionary<string,object> Deserialize(string jsonString)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ObjectAsPrimitiveConverter());
            return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString, options);
            
        }
    }
}
