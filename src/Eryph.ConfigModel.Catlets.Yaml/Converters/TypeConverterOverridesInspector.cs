using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryph.ConfigModel.Catlets;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Eryph.ConfigModel.Yaml.Converters;

/// <summary>
/// The only purpose of this <see cref="ITypeInspector"/> is to apply
/// the <see cref="FodderContentYamlTypeConverter"/> to the property
/// <see cref="FodderConfig.Content"/>.
/// </summary>
/// <remarks>
/// Unfortunately, <see cref="BuilderSkeleton{TBuilder}.WithAttributeOverride{TClass}"/>
/// cannot be used to apply a <see cref="YamlConverterAttribute"/> to a property. Hence,
/// we need to force our custom <see cref="FodderContentYamlTypeConverter"/> with this
/// <see cref="ITypeInspector"/>.
/// </remarks>
internal class TypeConverterOverridesInspector(
    ITypeInspector innerTypeInspector,
    IReadOnlyDictionary<(Type Type, string PropertyName), Type> converters)
    : TypeInspectorSkeleton
{
    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container) =>
        innerTypeInspector.GetProperties(type, container)
            .Select(pd => new TypeConverterOverridesPropertyDescriptor(
                pd, converters, type));

    public override string GetEnumName(Type enumType, string name) =>
        innerTypeInspector.GetEnumName(enumType, name);

    public override string GetEnumValue(object enumValue) =>
        innerTypeInspector.GetEnumValue(enumValue);

    private sealed class TypeConverterOverridesPropertyDescriptor(
        IPropertyDescriptor innerDescriptor,
        IReadOnlyDictionary<(Type Type, string PropertyName), Type> converters,
        Type classType)
        : IPropertyDescriptor
    {
        public string Name => innerDescriptor.Name;

        public bool AllowNulls => innerDescriptor.AllowNulls;

        public Type Type => innerDescriptor.Type;

        public Type? TypeOverride
        {
            get => innerDescriptor.TypeOverride;
            set => innerDescriptor.TypeOverride = value;
        }

        public int Order { get; set; }

        public ScalarStyle ScalarStyle
        {
            get => innerDescriptor.ScalarStyle;
            set => innerDescriptor.ScalarStyle = value;
        }

        public bool Required => innerDescriptor.Required;

        public Type? ConverterType =>
            converters.TryGetValue((classType, Name), out var converterType)
                ? converterType
                : innerDescriptor.ConverterType;

        public bool CanWrite => innerDescriptor.CanWrite;

        public void Write(object target, object? value) =>
            innerDescriptor.Write(target, value);

        public T? GetCustomAttribute<T>() where T : Attribute =>
            innerDescriptor.GetCustomAttribute<T>();

        public IObjectDescriptor Read(object target) =>
            innerDescriptor.Read(target);
    }
}
