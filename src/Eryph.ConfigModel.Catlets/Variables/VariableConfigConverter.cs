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
            Value = GetStringProperty(dictionary, nameof(VariableConfig.Value)),
            Secret = GetBoolProperty(dictionary, nameof(VariableConfig.Secret)),
            Required = GetBoolProperty(dictionary, nameof(VariableConfig.Required)),
        };
}
