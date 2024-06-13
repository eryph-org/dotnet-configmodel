using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.ConfigModel.Catlets;

public class CatletCapabilityConfig : IMutateableConfig<CatletCapabilityConfig>
{
    public string? Name { get; set; }
    public MutationType? Mutation { get; set; }
    public string[]? Details { get; set; }

    public CatletCapabilityConfig Clone()
    {
        return new CatletCapabilityConfig
        {
            Name = Name,
            Mutation = Mutation,
            Details = Details?.ToArray()
        };
    }

    internal static CatletCapabilityConfig[]? Breed(CatletConfig parentConfig, CatletConfig child)
    {
        return Breeding.WithMutation(parentConfig, child, x => x.Capabilities,
            (capability, childCap) =>
            {
                capability.Details = childCap?.Details ?? capability.Details;
            }
        );
    }
}
