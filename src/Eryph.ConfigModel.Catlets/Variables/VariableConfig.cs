using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryph.ConfigModel.Variables;

public class VariableConfig : ICloneableConfig<VariableConfig>
{
    public string? Name { get; set; }

    public VariableType? Type { get; set; }

    [PrivateIdentifier(Critical = true)]
    public string? Value { get; set; }

    public bool? Secret { get; set; }

    public bool? Required { get; set; }
    
    public VariableConfig Clone() => new()
    {
        Name = Name,
        Type = Type,
        Value = Value,
        Secret = Secret,
        Required = Required,
    };
}
