using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class VirtualCatletNetworkAdapterConfigConverter : DictionaryConverterBase<VirtualCatletNetworkAdapterConfig, CatletConfig>
    {
        public class List : DictionaryToListConverter<VirtualCatletNetworkAdapterConfig[], CatletConfig>
        {
            public List() : base(nameof(VirtualCatletConfig.NetworkAdapters))
            {
            }
        }
        public override VirtualCatletNetworkAdapterConfig ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object data = null)
        {
            return new VirtualCatletNetworkAdapterConfig
            {
                Name = GetStringProperty(dictionary, nameof(VirtualCatletNetworkAdapterConfig.Name)),
                MacAddress = GetStringProperty(dictionary, nameof(VirtualCatletNetworkAdapterConfig.MacAddress))
            };
        }
    }
    
}