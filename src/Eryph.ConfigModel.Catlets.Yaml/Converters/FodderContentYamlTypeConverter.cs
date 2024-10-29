using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Yaml.Converters;

internal class FodderContentYamlTypeConverter : IYamlTypeConverter
{
    // This converter is explicitly attached and hence can always return false
    public bool Accepts(Type type) => false;

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (parser is not StringParser customParser)
            throw new ArgumentException(
                $"This converter requires the {typeof(StringParser).FullName}",
                nameof(parser));

        if (parser.Accept<MappingStart>(out _))
            return customParser.ConsumeMappingAsString();
        
        if (parser.Accept<SequenceStart>(out _))
            return customParser.ConsumeSequenceAsString();

        return rootDeserializer(type);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer) =>
        serializer(value, type);
}
