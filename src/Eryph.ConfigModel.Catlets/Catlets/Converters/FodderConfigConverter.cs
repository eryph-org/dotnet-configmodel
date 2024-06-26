﻿using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;
using Eryph.ConfigModel.Variables;

namespace Eryph.ConfigModel.Catlets.Converters;

public class FodderConfigConverter<TConfig> : DictionaryConverterBase<FodderConfig, TConfig>
    where TConfig : IHasFodderConfig
{
    public class List() : DictionaryToListConverter<FodderConfig[], TConfig>(nameof(IHasFodderConfig.Fodder));

    public override FodderConfig ConvertFromDictionary(
        IConverterContext<TConfig> context, 
        IDictionary<object, object> dictionary,
        object? data = null) =>
        new()
        {
            Name = GetStringProperty(dictionary, nameof(FodderConfig.Name)),
            Source = GetStringProperty(dictionary, nameof(FodderConfig.Source)),
            Type = GetStringProperty(dictionary, nameof(FodderConfig.Type)),
            Content = GetStringProperty(dictionary, nameof(FodderConfig.Content)),
            FileName = GetStringProperty(dictionary, nameof(FodderConfig.FileName)),
            Secret = GetBoolProperty(dictionary, nameof(FodderConfig.Secret)),
            Remove = GetBoolProperty(dictionary, nameof(FodderConfig.Remove)),
            Variables = context.ConvertList<VariableConfig>(dictionary),
        };
}
