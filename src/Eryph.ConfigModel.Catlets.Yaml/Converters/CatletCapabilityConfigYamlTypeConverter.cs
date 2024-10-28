using System;
using Eryph.ConfigModel.Catlets;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Yaml.Converters;

internal class CatletCapabilityConfigYamlTypeConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(CatletCapabilityConfig);

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (!parser.TryConsume<Scalar>(out var scalar))
            return rootDeserializer(type);

        return new CatletCapabilityConfig
        {
            Name = scalar.Value,
        };
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        serializer(value, type);
    }
}
