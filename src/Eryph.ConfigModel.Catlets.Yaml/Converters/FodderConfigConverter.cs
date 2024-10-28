using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Text;
using Eryph.ConfigModel.Catlets;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Converters;

internal class FodderConfigConverter(INamingConvention namingConvention) : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(FodderConfig);

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (parser is not StringParser customParser)
            throw new ArgumentException("This converter requires a custom parser", nameof(parser));

        parser.Consume<MappingStart>();

        var result = new FodderConfig();

        while (parser.TryConsume<Scalar>(out var propertyName))
        {
            var property = typeof(FodderConfig).GetProperty(
                namingConvention.Reverse(propertyName.Value),
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property is null)
                throw new InvalidOperationException($"Unknown property {propertyName.Value}");

            if (property.Name == nameof(FodderConfig.Content))
            {
                if (parser.TryConsume<Scalar>(out var valueScalar))
                {
                    property.SetValue(result, valueScalar.Value);
                }
                else if (parser.Accept<MappingStart>(out _))
                {
                    var value = customParser.ConsumeMappingAsString();
                    property.SetValue(result, value);
                }
                else if (parser.Accept<SequenceStart>(out _))
                {
                    var value = customParser.ConsumeSequenceAsString();
                    property.SetValue(result, value);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown property value for {propertyName.Value}");
                }
            }
            else
            {
                var propertyValue = rootDeserializer(property.PropertyType);
                property.SetValue(result, propertyValue);
            }
        }

        parser.Consume<MappingEnd>();

        return result;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        serializer(value, type);
    }
}
