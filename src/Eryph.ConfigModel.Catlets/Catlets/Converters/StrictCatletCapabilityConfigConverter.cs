using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters;

public class StrictCatletCapabilityConfigConverter : DictionaryConverterBase<CatletCapabilityConfig, CatletConfig>
{
    public class List() : DictionaryToListConverter<CatletCapabilityConfig[], CatletConfig>(
        nameof(CatletConfig.Capabilities));
        
    public override CatletCapabilityConfig? ConvertFromDictionary(IConverterContext<CatletConfig> context, 
        IDictionary<object, object> dictionary, object? data = null)
    {
        return ConvertCapabilityConfigConfig(dictionary);
    }
        
    protected virtual CatletCapabilityConfig? ConvertCapabilityConfigConfig(object configObject)
    {
        if (configObject is not IDictionary<object, object> dictionary)
            throw new InvalidConfigModelException();
        
        return new CatletCapabilityConfig
        {
            Name = GetStringProperty(dictionary, nameof(CatletCapabilityConfig.Name)),
            Details = GetListProperty<string>(dictionary, nameof(CatletCapabilityConfig.Details)),
            Mutation = GetEnumProperty<MutationType>(dictionary, nameof(CatletCapabilityConfig.Mutation))
        };
    }
}
