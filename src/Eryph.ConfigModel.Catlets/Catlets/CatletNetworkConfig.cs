using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletNetworkConfig : IMutateableConfig<CatletNetworkConfig>, ICloneable
    {

        public string? Name { get; set; }
        public MutationType? Mutation { get; set; }
        public string? AdapterName { get; set; }
        public CatletSubnetConfig? SubnetV4 { get; set; }
        public CatletSubnetConfig? SubnetV6 { get; set; }

        public CatletNetworkConfig Clone()
        {
            return new CatletNetworkConfig
            {
                Name = Name,
                Mutation = Mutation,
                AdapterName = AdapterName,
                SubnetV4 = SubnetV4?.Clone(),
                SubnetV6 = SubnetV6?.Clone()
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        internal static CatletNetworkConfig[]? Breed(CatletConfig parentConfig, CatletConfig child)
        {
            return Breeding.WithMutation(parentConfig, child, x => x.Networks,
                (network, childNetwork) =>
                {
                    network.AdapterName = childNetwork.AdapterName ?? network.AdapterName;
                    network.SubnetV4 = childNetwork.SubnetV4?.Clone() ?? network.SubnetV4;
                    network.SubnetV6 = childNetwork.SubnetV6?.Clone() ?? network.SubnetV6;
                }
            );
        }
    }
}