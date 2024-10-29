using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Yaml.Converters;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace Eryph.ConfigModel.Yaml;

public static class CatletConfigYamlSerializer
{
    private static readonly Lazy<IDeserializer> Deserializer = new(() =>
        new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithTypeConverter(new CatletCapabilityConfigYamlTypeConverter(
                UnderscoredNamingConvention.Instance))
            .WithTypeConverter(new CatletCpuConfigYamlTypeConverter(
                UnderscoredNamingConvention.Instance))
            .WithTypeConverter(new CatletMemoryConfigYamlTypeConverter(
                UnderscoredNamingConvention.Instance))
            .WithTypeConverter(new FodderContentYamlTypeConverter())
            .WithTypeInspector(
                ti => new TypeConverterOverridesInspector(
                    ti,
                    new Dictionary<(Type Type, string PropertyName), Type>
                    {
                        [(typeof(FodderConfig), nameof(FodderConfig.Content))] = typeof(FodderContentYamlTypeConverter),
                    }),
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
