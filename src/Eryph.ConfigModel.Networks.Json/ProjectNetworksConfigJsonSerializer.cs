using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Eryph.ConfigModel.Networks;

namespace Eryph.ConfigModel.Json;

public static class ProjectNetworksConfigJsonSerializer
{
    private static readonly Lazy<JsonSerializerOptions> LazyOptions = new(() =>
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() },
        });

    public static JsonSerializerOptions Options => LazyOptions.Value;

    public static ProjectNetworksConfig Deserialize(JsonElement json) =>
        json.Deserialize<ProjectNetworksConfig>(Options)
            ?? throw new JsonException("The config must not be null.");

    public static ProjectNetworksConfig Deserialize(string json) =>
        JsonSerializer.Deserialize<ProjectNetworksConfig>(json, Options)
            ?? throw new JsonException("The config must not be null.");

    public static string Serialize(
        ProjectNetworksConfig config,
        JsonSerializerOptions? options = default) =>
        JsonSerializer.Serialize(config, options ?? Options);

    public static JsonElement SerializeToElement(
        ProjectNetworksConfig config,
        JsonSerializerOptions? options = default) =>
        JsonSerializer.SerializeToElement(config, options ?? Options);
}
