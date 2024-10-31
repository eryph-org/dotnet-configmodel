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
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            Converters = { new JsonStringEnumConverter() },
        });

    public static JsonSerializerOptions Options => LazyOptions.Value;

    public static CatletConfig Deserialize(JsonElement json)
    {
        try
        {
            return json.Deserialize<CatletConfig>(Options)
                   ?? throw new JsonException("The config must not be null.");
        }
        catch (Exception ex)
        {
            throw InvalidConfigExceptionFactory.Create(ex);
        }
    }

    public static CatletConfig Deserialize(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<CatletConfig>(json, Options)
                   ?? throw new JsonException("The config must not be null.");
        }
        catch (Exception ex)
        {
            throw InvalidConfigExceptionFactory.Create(ex);
        }
    }

    public static string Serialize(
        CatletConfig config,
        JsonSerializerOptions? options = default) =>
        JsonSerializer.Serialize(config, options ?? Options);

    public static JsonElement SerializeToElement(
        CatletConfig config,
        JsonSerializerOptions? options = default) =>
        JsonSerializer.SerializeToElement(config, options ?? Options);
}
