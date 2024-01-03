using System;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletNetworkAdapterConfig: IMutateableConfig<CatletNetworkAdapterConfig>, ICloneable
    {
        public string? Name { get; set; }
        public MutationType? Mutation { get; set; }
        
        public string? MacAddress { get; set; }

        public CatletNetworkAdapterConfig Clone()
        {
            return new CatletNetworkAdapterConfig
            {
                Name = Name,
                Mutation = Mutation,
                MacAddress = MacAddress
            };
        }
        
        object ICloneable.Clone()
        {
            return Clone();
        }

        internal static CatletNetworkAdapterConfig[]? Breed(CatletConfig parentConfig, CatletConfig child)
        {
            return Breeding.WithMutation(parentConfig, child, x => x.NetworkAdapters,
                (adapter, childAdapter) =>
                {
                    adapter.MacAddress = childAdapter?.MacAddress ?? adapter.MacAddress;
                }
            );
            
        }
    }
}

