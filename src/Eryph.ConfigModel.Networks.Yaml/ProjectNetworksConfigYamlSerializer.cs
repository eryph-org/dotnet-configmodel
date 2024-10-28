using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Networks;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ConfigModel.Yaml;

public static class ProjectNetworksConfigYamlSerializer
{
    private static readonly Lazy<IDeserializer> Deserializer = new(() =>
        new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build());

    private static readonly Lazy<ISerializer> Serializer = new(() =>
        new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .Build());

    public static ProjectNetworksConfig Deserialize(string yaml) =>
        Deserializer.Value.Deserialize<ProjectNetworksConfig>(yaml);

    public static string Serialize(ProjectNetworksConfig config) =>
        Serializer.Value.Serialize(config);
}
