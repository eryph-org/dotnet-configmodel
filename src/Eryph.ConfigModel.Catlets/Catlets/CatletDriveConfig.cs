using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets;

[PublicAPI]
public class CatletDriveConfig : IMutateableConfig<CatletDriveConfig>
{
    public string? Name { get; set; }
        
    public MutationType? Mutation { get; set; }
        
    public string? Location { get; set; }

    public string? Store { get; set; }

    [PrivateIdentifier]
    public string? Source { get; set; }

    public int? Size { get; set; }

    public CatletDriveType? Type { get; set; }

    public CatletDriveConfig Clone() => new()
    {
        Name = Name,
        Mutation = Mutation,
        Location = Location,
        Store = Store,
        Source = Source,
        Size = Size,
        Type = Type
    };
}
