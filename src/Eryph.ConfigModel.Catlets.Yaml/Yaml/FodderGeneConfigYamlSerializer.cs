using System.Collections.Generic;
using Eryph.ConfigModel.FodderGenes;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Yaml;

public static class FodderGeneConfigYamlSerializer
{
    private static ISerializer? _serializer;
    private static IDeserializer? _deSerializer;

    public static string Serialize(FodderGeneConfig config)
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

    public static FodderGeneConfig Deserialize(string yaml)
    {
        if (_deSerializer == null)
            _deSerializer = new DeserializerBuilder()
                .Build();

        var dictionary = _deSerializer.Deserialize<Dictionary<object, object>>(yaml);
        return FodderConfigDictionaryConverter.Convert(dictionary, true);
    }
}