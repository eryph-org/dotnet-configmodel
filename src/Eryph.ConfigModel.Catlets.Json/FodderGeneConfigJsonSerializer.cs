using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Eryph.ConfigModel.FodderGenes;

namespace Eryph.ConfigModel.Json;

public static class FodderGeneConfigJsonSerializer
{
    private static readonly Lazy<JsonSerializerOptions> LazyOptions = new(() =>
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() },
        });

    public static JsonSerializerOptions Options => LazyOptions.Value;

    public static string Serialize(FodderGeneConfig config, JsonSerializerOptions? options = default) =>
        JsonSerializer.Serialize(config, options ?? Options);

    public static FodderGeneConfig? Deserialize(JsonElement json) =>
        json.Deserialize<FodderGeneConfig>(Options);

    public static FodderGeneConfig? Deserialize(string json) =>
        JsonSerializer.Deserialize<FodderGeneConfig>(json, Options);
}
