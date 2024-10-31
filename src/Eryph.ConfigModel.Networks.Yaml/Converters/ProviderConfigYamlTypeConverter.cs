using System;
using System.Collections.Generic;
using System.Text;
using Eryph.ConfigModel.Networks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Yaml.Converters;

internal class ProviderConfigYamlTypeConverter(
    ITypeInspector typeInspector)
    : YamlTypeConverterBase<ProviderConfig>(typeInspector)
{
    public override object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (!parser.TryConsume<Scalar>(out var scalar))
            return base.ReadYaml(parser, type, rootDeserializer);

        return new ProviderConfig
        {
            Name = scalar.Value,
        };
    }
}
