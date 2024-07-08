using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.ConfigModel.Catlets;

public class CatletCapabilityConfig : IMutateableConfig<CatletCapabilityConfig>
{
    public string? Name { get; set; }

    public MutationType? Mutation { get; set; }

    public string[]? Details { get; set; }

    public CatletCapabilityConfig Clone() => new()
    {
        Name = Name,
        Mutation = Mutation,
        Details = Details?.ToArray()
    };
}
