using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters;

public class CatletNetworkAdapterConfigConverter : DictionaryConverterBase<CatletNetworkAdapterConfig, CatletConfig>
{
    public class List() : DictionaryToListConverter<CatletNetworkAdapterConfig[], CatletConfig>(
        nameof(CatletConfig.NetworkAdapters));

    public override CatletNetworkAdapterConfig ConvertFromDictionary(
        IConverterContext<CatletConfig> context, 
        IDictionary<object, object> dictionary,
        object? data = null)
    {
        return new CatletNetworkAdapterConfig
        {
            Name = GetStringProperty(dictionary, nameof(CatletNetworkAdapterConfig.Name)),
            MacAddress = GetStringProperty(dictionary, nameof(CatletNetworkAdapterConfig.MacAddress)),
            Mutation = GetEnumProperty<MutationType>(dictionary, nameof(CatletNetworkAdapterConfig.Mutation))
        };
    }
}
