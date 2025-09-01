using System;
using Eryph.ConfigModel.Networks;
using Eryph.ConfigModel.Yaml.Converters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Yaml;

public static class ProjectNetworksConfigYamlSerializer
{
    private static readonly Lazy<IDeserializer> Deserializer = new(() =>
    {
        var builder = new DeserializerBuilder()
            .WithCaseInsensitivePropertyMatching()
            .WithEnumNamingConvention(UnderscoredNamingConvention.Instance)
            .WithNamingConvention(UnderscoredNamingConvention.Instance);

        // Build the type inspector first as some of our type converters require it.
        // You must update YamlTypeConverterBase accordingly if you change the
        // configuration of the deserializer.
        var typeInspector = builder.BuildTypeInspector();

        return builder
            .WithTypeConverter(new ProviderConfigYamlTypeConverter(typeInspector))
            .Build();
    });

    private static readonly Lazy<ISerializer> Serializer = new(() =>
        new SerializerBuilder()
            .WithEnumNamingConvention(UnderscoredNamingConvention.Instance)
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .DisableAliases()
            .Build());

    public static ProjectNetworksConfig Deserialize(string yaml)
    {
        try
        {
            return Deserializer.Value.Deserialize<ProjectNetworksConfig>(yaml);
        }
        catch (Exception ex)
        {
            throw InvalidConfigExceptionFactory.Create(ex);
        }
    }

    public static string Serialize(ProjectNetworksConfig config) =>
        Serializer.Value.Serialize(config);
}
