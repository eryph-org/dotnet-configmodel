using Eryph.ConfigModel.Catlets;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Eryph.ConfigModel.Yaml.Converters;

internal abstract class ReflectionYamlTypeConverterBase<T>(
    INamingConvention namingConvention)
    : IYamlTypeConverter
    where T : class, new()
{
    public bool Accepts(Type type) => type == typeof(T);

    public virtual object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        parser.Consume<MappingStart>();

        var result = new T();

        while (parser.TryConsume<Scalar>(out var propertyName))
        {
            var property = typeof(T).GetProperty(
                namingConvention.Reverse(propertyName.Value),
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property is null)
                throw new InvalidOperationException($"Unknown property {propertyName.Value}");

            var propertyValue = rootDeserializer(property.PropertyType);
            property.SetValue(result, propertyValue);
        }

        parser.Consume<MappingEnd>();

        return result;
    }

    public virtual void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        serializer(value, type);
    }
}
