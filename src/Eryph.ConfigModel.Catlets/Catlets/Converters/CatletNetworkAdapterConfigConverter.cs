using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class CatletNetworkAdapterConfigConverter : DictionaryConverterBase<CatletNetworkAdapterConfig, CatletConfig>
    {
        public class List : DictionaryToListConverter<CatletNetworkAdapterConfig[], CatletConfig>
        {
            public List() : base(nameof(CatletConfig.NetworkAdapters))
            {
            }
        }
        public override CatletNetworkAdapterConfig ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object? data = null)
        {
            var mutationString = GetStringProperty(dictionary, nameof(CatletNetworkAdapterConfig.Mutation));
            MutationType? mutation = null;
            if (mutationString != null)
            {
                if (Enum.TryParse(mutationString, true, out MutationType mutationOut))
                    mutation = mutationOut;
            }
            return new CatletNetworkAdapterConfig
            {
                Name = GetStringProperty(dictionary, nameof(CatletNetworkAdapterConfig.Name)),
                MacAddress = GetStringProperty(dictionary, nameof(CatletNetworkAdapterConfig.MacAddress)),
                Mutation = mutation
            };
        }
    }
    
}