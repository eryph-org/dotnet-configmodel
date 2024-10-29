using System;
using Eryph.ConfigModel.Catlets;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Yaml.Converters;

internal class CatletMemoryConfigYamlTypeConverter(
    INamingConvention namingConvention)
    : ReflectionYamlTypeConverterBase<CatletMemoryConfig>(namingConvention)
{
    public override object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (!parser.Accept<Scalar>(out _))
            return base.ReadYaml(parser, type, rootDeserializer);

        var startup = (int?)rootDeserializer(typeof(int));
        return new CatletMemoryConfig()
        {
            Startup = startup,
        };
    }
}
