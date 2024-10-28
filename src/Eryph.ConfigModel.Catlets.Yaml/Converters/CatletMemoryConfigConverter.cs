using System;
using System.Collections.Generic;
using System.Text;
using Eryph.ConfigModel.Catlets;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Converters;

internal class CatletMemoryConfigConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(CatletMemoryConfig);

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (!parser.Accept<Scalar>(out _))
        {
            return rootDeserializer(type);
        }

        var startup = (int)rootDeserializer(typeof(int))!;
        return new CatletMemoryConfig()
        {
            Startup = startup,
        };
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        serializer(value, type);
    }
}