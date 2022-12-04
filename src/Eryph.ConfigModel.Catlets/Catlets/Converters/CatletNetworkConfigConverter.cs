using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class CatletNetworkConfigConverter : DictionaryConverterBase<CatletNetworkConfig, CatletConfig>
    {
        public class List : DictionaryToListConverter<CatletNetworkConfig[], CatletConfig>
        {
            public List() : base(nameof(CatletConfig.Networks))
            {
            }
        }

        public override CatletNetworkConfig ConvertFromDictionary(
            IConverterContext<CatletConfig> context, IDictionary<object, object> dictionary, object data = default)
        {
            return new CatletNetworkConfig
            {
                Name = GetStringProperty(dictionary, nameof(CatletNetworkConfig.Name)),
                AdapterName = GetStringProperty(dictionary, nameof(CatletNetworkConfig.AdapterName), "adapter_name"),
                SubnetV4 = context.Convert<CatletSubnetConfig>(dictionary, new [] {nameof(CatletNetworkConfig.SubnetV4), "subnet"}),
                SubnetV6 = context.Convert<CatletSubnetConfig>(dictionary, new [] { nameof(CatletNetworkConfig.SubnetV6) })
            };
        }

    }
}