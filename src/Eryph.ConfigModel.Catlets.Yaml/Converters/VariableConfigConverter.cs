using System;
using System.Collections.Generic;
using System.Text;
using Eryph.ConfigModel.Variables;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Converters;

internal class VariableConfigConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(VariableConfig);

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        return null;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        serializer(value, type);
    }
}