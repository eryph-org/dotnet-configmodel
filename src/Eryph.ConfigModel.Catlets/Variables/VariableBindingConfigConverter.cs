using System;
using System.Collections.Generic;
using System.Text;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Variables;

public class VariableBindingConfigConverter<TConfig>
    : DictionaryConverterBase<VariableBindingConfig, TConfig>
    where TConfig : IHasFodderConfig
{
    public class List() : DictionaryToListConverter<VariableBindingConfig[], TConfig>(nameof(FodderConfig.Variables));

    public override VariableBindingConfig ConvertFromDictionary(
        IConverterContext<TConfig> context,
        IDictionary<object, object> dictionary,
        object? data = default) =>
        new()
        {
            Name = GetStringProperty(dictionary, nameof(VariableBindingConfig.Name)),
            Value = GetStringProperty(dictionary, nameof(VariableBindingConfig.Value)),
            Secret = GetBoolProperty(dictionary, nameof(VariableBindingConfig.Secret)),
        };
}
