using System;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets;

[PublicAPI]
public class CatletNetworkAdapterConfig : IMutateableConfig<CatletNetworkAdapterConfig>
{
    public string? Name { get; set; }

    public MutationType? Mutation { get; set; }
        
    public string? MacAddress { get; set; }

    public CatletNetworkAdapterConfig Clone() => new()
    {
        Name = Name,
        Mutation = Mutation,
        MacAddress = MacAddress
    };
}
