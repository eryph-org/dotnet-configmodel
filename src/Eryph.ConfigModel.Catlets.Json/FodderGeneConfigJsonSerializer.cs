﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.FodderGenes;

namespace Eryph.ConfigModel.Json;

public static class FodderGeneConfigJsonSerializer
{
    private static readonly Lazy<JsonSerializerOptions> LazyOptions = new(() =>
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() },
        });

    public static JsonSerializerOptions Options => LazyOptions.Value;

    public static FodderGeneConfig Deserialize(JsonElement json) =>
        json.Deserialize<FodderGeneConfig>(Options)
            ?? throw new JsonException("The config must not be null.");

    public static FodderGeneConfig Deserialize(string json) =>
        JsonSerializer.Deserialize<FodderGeneConfig>(json, Options)
            ?? throw new JsonException("The config must not be null.");

    public static string Serialize(
        FodderGeneConfig config,
        JsonSerializerOptions? options = default) =>
        JsonSerializer.Serialize(config, options ?? Options);

    public static JsonElement SerializeToElement(
        FodderGeneConfig config,
        JsonSerializerOptions? options = default) =>
        JsonSerializer.SerializeToElement(config, options ?? Options);
}
