using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets;

[PublicAPI]
public class CatletNetworkConfig : IMutateableConfig<CatletNetworkConfig>
{
    public string? Name { get; set; }

    public MutationType? Mutation { get; set; }

    public string? AdapterName { get; set; }

    public CatletSubnetConfig? SubnetV4 { get; set; }

    public CatletSubnetConfig? SubnetV6 { get; set; }

    public CatletNetworkConfig Clone() => new()
    {
        Name = Name,
        Mutation = Mutation,
        AdapterName = AdapterName,
        SubnetV4 = SubnetV4?.Clone(),
        SubnetV6 = SubnetV6?.Clone()
    };
}
