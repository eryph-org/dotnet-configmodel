using System;
using System.Text.Json;

namespace Eryph.ConfigModel.Json;

public static class InvalidConfigExceptionFactory
{
    public static InvalidConfigException Create(Exception exception) =>
        exception is JsonException jsonException
        ? new InvalidConfigException(
            $"The JSON is invalid (line {(jsonException.LineNumber ?? 0) + 1}, column {(jsonException.BytePositionInLine ?? 0) + 1}):\n"
            + exception.Message,
            exception)
        : new InvalidConfigException(
            $"The JSON is invalid:\n{exception.Message}",
            exception);
}
