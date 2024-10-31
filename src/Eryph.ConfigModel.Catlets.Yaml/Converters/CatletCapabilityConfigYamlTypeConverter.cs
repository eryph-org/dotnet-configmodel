using System;
using Eryph.ConfigModel.Catlets;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Yaml.Converters;

internal class CatletCapabilityConfigYamlTypeConverter(
    ITypeInspector typeInspector)
    : YamlTypeConverterBase<CatletCapabilityConfig>(typeInspector)
{
    public override object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (!parser.TryConsume<Scalar>(out var scalar))
            return base.ReadYaml(parser, type, rootDeserializer);

        return new CatletCapabilityConfig
        {
            Name = scalar.Value,
        };
    }
}
