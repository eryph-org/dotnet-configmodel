using System.Collections.Generic;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Converters;
using Eryph.ConfigModel.FodderGenes;
using YamlDotNet.Core;
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
                .WithAttributeOverride<FodderConfig>(c => c.Content!, new YamlMemberAttribute { ScalarStyle = ScalarStyle.Literal})
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
                .Build();
        }

        return _serializer.Serialize(config);
    }

    public static FodderGeneConfig Deserialize(string yaml)
    {
        if (_deSerializer == null)
        {
            var builder = new DeserializerBuilder();
            _deSerializer = builder
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .WithTypeConverter(new FodderConfigConverter(UnderscoredNamingConvention.Instance))
                .Build();
        }

        return _deSerializer.Deserialize<FodderGeneConfig>(new StringParser(yaml));
    }
}
