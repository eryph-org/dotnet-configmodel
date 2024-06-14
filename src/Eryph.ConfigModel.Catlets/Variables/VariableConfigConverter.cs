using System;
using System.Collections.Generic;
using System.Text;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Variables;

public class VariableConfigConverter<TConfig> : DictionaryConverterBase<VariableConfig, TConfig>
    where TConfig : IHasVariableConfig
{
    public class List() : DictionaryToListConverter<VariableConfig[], TConfig>(nameof(IHasVariableConfig.Variables));

    public override VariableConfig ConvertFromDictionary(
        IConverterContext<TConfig> context,
        IDictionary<object, object> dictionary,
        object? data = null) =>
        new()
        {
            Name = GetStringProperty(dictionary, nameof(VariableConfig.Name)),
            Type = GetEnumProperty<VariableType>(dictionary, nameof(VariableConfig.Type)),
            Value = GetVariableValue(dictionary),
            Secret = GetBoolProperty(dictionary, nameof(VariableConfig.Secret)),
            Required = GetBoolProperty(dictionary, nameof(VariableConfig.Required)),
        };

    private string? GetVariableValue(IDictionary<object, object> dictionary)
    {
        var value = GetValueCaseInvariant(dictionary, nameof(VariableConfig.Value));
        return value is bool
            ? value.ToString().ToLowerInvariant()
            : value?.ToString().TrimEnd(char.MinValue);
    }
}
