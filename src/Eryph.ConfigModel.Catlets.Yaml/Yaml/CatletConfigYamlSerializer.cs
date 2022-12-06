using System.Collections.Generic;
using Eryph.ConfigModel.Catlets;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Yaml
{
    public static class CatletConfigYamlSerializer
    {
        private static ISerializer _serializer; 
        private static IDeserializer _deSerializer; 
        
        public static string Serialize(CatletConfig config)
        {
            if (_serializer == null)
            {
                _serializer = new SerializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .WithAttributeOverride<CatletConfig>(d => d.VCatlet,
                        new YamlMemberAttribute
                        {
                            Alias = "vcatlet",
                            ApplyNamingConventions = false
                        })
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
                    .Build();
            }

            return _serializer.Serialize(config);
        }

        public static CatletConfig Deserialize(string yaml)
        {
            if(_deSerializer == null)
                _deSerializer = new DeserializerBuilder()
                .Build();
          
            var dictionary = _deSerializer.Deserialize<Dictionary<object, object>>(yaml);
            return CatletConfigDictionaryConverter.Convert(dictionary, true);
        }
    }
}