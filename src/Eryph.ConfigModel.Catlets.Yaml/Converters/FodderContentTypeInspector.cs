using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryph.ConfigModel.Catlets;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Eryph.ConfigModel.Yaml.Converters;

internal class FodderContentTypeInspector(ITypeInspector innerTypeDescriptor)
    : TypeInspectorSkeleton
{
    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container) =>
        type != typeof(FodderConfig)
            ? innerTypeDescriptor.GetProperties(type, container)
            : innerTypeDescriptor.GetProperties(type, container)
                .Select(pd => pd.Name == nameof(FodderConfig.Content)
                    ? new FodderContentPropertyDescriptor(pd, typeof(FodderContentYamlTypeConverter))
                    : pd);


    public override string GetEnumName(Type enumType, string name) =>
        innerTypeDescriptor.GetEnumName(enumType, name);

    public override string GetEnumValue(object enumValue) =>
        innerTypeDescriptor.GetEnumValue(enumValue);
}

internal class FodderContentPropertyDescriptor(
    IPropertyDescriptor innerDescriptor,
    Type converterType)
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

    public Type? ConverterType => converterType;

    public bool CanWrite => innerDescriptor.CanWrite;

    public void Write(object target, object? value) =>
        innerDescriptor.Write(target, value);

    public T? GetCustomAttribute<T>() where T : Attribute =>
        innerDescriptor.GetCustomAttribute<T>();

    public IObjectDescriptor Read(object target) =>
        innerDescriptor.Read(target);
}
