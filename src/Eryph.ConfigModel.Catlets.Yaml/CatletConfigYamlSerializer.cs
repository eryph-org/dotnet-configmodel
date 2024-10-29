using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Yaml.Converters;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Yaml;

public static class CatletConfigYamlSerializer
{
    private static readonly Lazy<IDeserializer> Deserializer = new(() =>
        new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithTypeConverter(new CatletCapabilityConfigYamlTypeConverter())
            .WithTypeConverter(new CatletCpuConfigYamlTypeConverter())
            .WithTypeConverter(new CatletMemoryConfigYamlTypeConverter())
            .WithTypeConverter(new FodderContentYamlTypeConverter())
            .WithTypeInspector(
                ti => new FodderContentTypeInspector(ti),
                where => where.OnBottom())
            .Build());

    private static readonly Lazy<ISerializer> Serializer = new(() =>
        new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithAttributeOverride<FodderConfig>(
                c => c.Content!,
                new YamlMemberAttribute { ScalarStyle = ScalarStyle.Literal })
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .Build());

    public static CatletConfig Deserialize(string yaml) =>
        Deserializer.Value.Deserialize<CatletConfig>(new StringParser(yaml));

    public static string Serialize(CatletConfig config) =>
        Serializer.Value.Serialize(config);
}
