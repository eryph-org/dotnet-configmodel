using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Eryph.ConfigModel.Catlets;

namespace Eryph.ConfigModel.Json;

public static class CatletConfigJsonSerializer
{
    private static readonly Lazy<JsonSerializerOptions> LazyOptions = new(() => 
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() },
        });

    public static JsonSerializerOptions Options => LazyOptions.Value;

    public static CatletConfig Deserialize(JsonElement json) =>
        json.Deserialize<CatletConfig>(Options)
            ?? throw new JsonException("The config must not be null.");

    public static CatletConfig Deserialize(string json) =>
        JsonSerializer.Deserialize<CatletConfig>(json, Options)
            ?? throw new JsonException("The config must not be null.");

    public static string Serialize(
        CatletConfig config,
        JsonSerializerOptions? options = default) =>
        JsonSerializer.Serialize(config, options ?? Options);

    public static JsonElement SerializeToElement(
        CatletConfig config,
        JsonSerializerOptions? options = default) =>
        JsonSerializer.SerializeToElement(config, options ?? Options);
}
