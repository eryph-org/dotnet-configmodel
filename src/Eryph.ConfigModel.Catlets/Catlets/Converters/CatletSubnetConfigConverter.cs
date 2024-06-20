using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters;

public class CatletSubnetConfigConverter : DictionaryConverterBase<CatletSubnetConfig, CatletConfig>
{
    public override CatletSubnetConfig? ConvertFromDictionary(
        IConverterContext<CatletConfig> context,
        IDictionary<object, object> dictionary, 
        object? data = null)
    {
        var propertyNames = data as string[] ?? [nameof(CatletNetworkConfig.SubnetV4)];
            
        var config = ConvertDictionary(GetValueCaseInvariant(dictionary, propertyNames));
        return config == null ? null : ConvertSubnetConfig(config);
    }

    protected virtual CatletSubnetConfig ConvertSubnetConfig(object configObject)
    {
        if (configObject is not IDictionary<object, object> dictionary)
            throw new InvalidConfigModelException();
        
        return new CatletSubnetConfig
        {
            Name = GetStringProperty(dictionary, nameof(CatletSubnetConfig.Name)),
            IpPool = GetStringProperty(dictionary, nameof(CatletSubnetConfig.IpPool)),
        };
    }
}
