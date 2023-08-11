using System.Collections.Generic;
using Eryph.ConfigModel.Networks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Yaml
{
    public static class ProjectNetworkConfigYamlSerializer
    {
        private static ISerializer _serializer; 
        private static IDeserializer _deSerializer; 
        
        public static string Serialize(ProjectNetworksConfig config)
        {
            if (_serializer == null)
            {
                _serializer = new SerializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
                    .Build();
            }

            return _serializer.Serialize(config);
        }

        public static ProjectNetworksConfig Deserialize(string yaml)
        {
            if(_deSerializer == null)
                _deSerializer = new DeserializerBuilder()
                .Build();
          
            var dictionary = _deSerializer.Deserialize<Dictionary<object, object>>(yaml);
            return ProjectNetworksConfigDictionaryConverter.Convert(dictionary, true);
        }
    }
}