using System;
using System.Collections.Generic;
using System.Text;
using Eryph.ConfigModel.Catlets;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Yaml.Converters;

internal class CatletCpuConfigYamlTypeConverter(
    INamingConvention namingConvention)
    : ReflectionYamlTypeConverterBase<CatletCpuConfig>(namingConvention)
{
    public override object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (!parser.Accept<Scalar>(out _))
            return base.ReadYaml(parser, type, rootDeserializer);

        var cpuCount = (int?)rootDeserializer(typeof(int));
        return new CatletCpuConfig
        {
            Count = cpuCount,
        };
    }
}
