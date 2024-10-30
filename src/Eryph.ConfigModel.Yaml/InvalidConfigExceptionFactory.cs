using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Core;

namespace Eryph.ConfigModel.Yaml;

public static class InvalidConfigExceptionFactory
{
    public static InvalidConfigException Create(Exception exception) =>
        exception is YamlException yamlException
            ? new InvalidConfigException(
                $"The YAML is invalid (line {yamlException.Start.Line}, column {yamlException.Start.Column}):\n"
                + $"{yamlException.Message}\n"
                + $"Make sure to use snake case for names, e.g. 'network_adapters'.",
                exception)
            : new InvalidConfigException(
                $"The YAML is invalid:\n{exception.Message}",
                exception);
}
