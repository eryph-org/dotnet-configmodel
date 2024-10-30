using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Yaml.Converters;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Yaml;

public static class FodderGeneConfigYamlSerializer
{
    private static readonly Lazy<IDeserializer> Deserializer = new(() => 
        new DeserializerBuilder()
            .WithCaseInsensitivePropertyMatching()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
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

    public static FodderGeneConfig Deserialize(string yaml)
    {
        try
        {
            return Deserializer.Value.Deserialize<FodderGeneConfig>(new StringParser(yaml));
        }
        catch (Exception ex)
        {
            throw InvalidConfigExceptionFactory.Create(ex);
        }
    }

    public static string Serialize(FodderGeneConfig config) =>
        Serializer.Value.Serialize(config);
}
