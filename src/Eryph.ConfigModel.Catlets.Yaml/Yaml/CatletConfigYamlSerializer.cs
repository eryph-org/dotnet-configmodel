using System.Collections.Generic;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Converters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Yaml;

public static class CatletConfigYamlSerializer
{
    private static ISerializer? _serializer; 
    private static IDeserializer? _deserializer; 
        
    public static string Serialize(CatletConfig config)
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

    public static CatletConfig Deserialize(string yaml)
    {
        if (_deserializer == null)
        {
            var builder = new DeserializerBuilder();
            _deserializer = builder
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .WithTypeConverter(new CatletCapabilityConfigYamlConverter())
                .WithTypeConverter(new CatletCpuConfigConverter())
                .WithTypeConverter(new CatletMemoryConfigConverter())
                .WithTypeConverter(new FodderConfigConverter(UnderscoredNamingConvention.Instance))
                .Build();
        }
          
        return _deserializer.Deserialize<CatletConfig>(new StringParser(yaml));
    }
}
