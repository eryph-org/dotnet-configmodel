using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Yaml.Converters;

internal abstract class YamlTypeConverterBase<T>(
    ITypeInspector typeInspector)
    : IYamlTypeConverter
    where T : class, new()
{
    public bool Accepts(Type type) => type == typeof(T);

    public virtual object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        parser.Consume<MappingStart>();

        var result = new T();

        while (!parser.TryConsume<MappingEnd>(out _))
        {
            var propertyName = parser.Consume<Scalar>();

            IPropertyDescriptor propertyDescriptor;
            try
            {
                // GetProperty throws SerializationException if the property is
                // not found as ignoreUnmatched is false.
                propertyDescriptor = typeInspector.GetProperty(typeof(T), null, propertyName.Value, false, true);
            }
            catch (SerializationException ex)
            {
                // Convert the SerializationException thrown by ITypeConverter in
                // the same way as YamlDotNet does.
                throw new YamlException(propertyName.Start, propertyName.End, ex.Message);
            }

            var propertyValue = rootDeserializer(propertyDescriptor.Type);
            propertyDescriptor.Write(result, propertyValue);
        }

        return result;
    }

    public virtual void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        serializer(value, type);
    }
}
