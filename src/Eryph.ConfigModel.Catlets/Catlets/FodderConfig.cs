using System;
using System.Collections.Generic;
using System.Linq;
using Eryph.ConfigModel.Variables;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets;

[PublicAPI]
public class FodderConfig: ICloneableConfig<FodderConfig>, IHasVariableConfig
{
    public string? Name { get; set; }

    public bool? Remove { get; set; }
        
    public string? Source { get; set; }
    
    public string? Type { get; set; }
        
    [PrivateIdentifier(Critical = true)]
    public string? Content { get; set; }
    
    public string? Filename { get; set; }
        
    public bool? Secret { get; set; }

    public VariableConfig[]? Variables { get; set; }

    public FodderConfig Clone() => new()
    {
        Name = Name,
        Remove = Remove,
        Source = Source,
        Type = Type,
        Content = Content,
        Filename = Filename,
        Secret = Secret,
        Variables = Variables?.Select(x => x.Clone()).ToArray(),
    };
}
