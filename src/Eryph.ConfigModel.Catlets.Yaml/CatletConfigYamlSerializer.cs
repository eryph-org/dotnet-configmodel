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
    {
        var builder = new DeserializerBuilder()
            .WithCaseInsensitivePropertyMatching()
            .WithEnumNamingConvention(UnderscoredNamingConvention.Instance)
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithAttributeOverride<FodderConfig>(
                c => c.Content!,
                new YamlConverterAttribute(typeof(FodderContentYamlTypeConverter)));

        // Build the type inspector first as some of our type converters require it.
        // You must update YamlTypeConverterBase accordingly if you change the
        // configuration of the deserializer.
        var typeInspector = builder.BuildTypeInspector();

        return builder
            .WithTypeConverter(new CatletCapabilityConfigYamlTypeConverter(typeInspector))
            .WithTypeConverter(new CatletCpuConfigYamlTypeConverter(typeInspector))
            .WithTypeConverter(new CatletMemoryConfigYamlTypeConverter(typeInspector))
            .WithTypeConverter(new FodderContentYamlTypeConverter())
            .Build();
    });

    private static readonly Lazy<ISerializer> Serializer = new(() =>
        new SerializerBuilder()
            .WithEnumNamingConvention(UnderscoredNamingConvention.Instance)
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithAttributeOverride<FodderConfig>(
                c => c.Content!,
                new YamlMemberAttribute { ScalarStyle = ScalarStyle.Literal })
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .DisableAliases()
            .Build());

    public static CatletConfig Deserialize(string yaml)
    {
        try
        {
            return Deserializer.Value.Deserialize<CatletConfig>(new StringParser(yaml));
        }
        catch (Exception ex)
        {
            throw InvalidConfigExceptionFactory.Create(ex);
        }
    }
    
    public static string Serialize(CatletConfig config) =>
        Serializer.Value.Serialize(config);
}
