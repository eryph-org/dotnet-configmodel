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
    
    public VariableConfig Clone() =>
        new()
        {
            Name = Name,
            Type = Type,
            Value = Value,
            Secret = Secret,
            Required = Required,
        };

    internal static VariableConfig[]? Breed(IHasVariableConfig parentConfig, IHasVariableConfig child)
    {
        var parentVariables = parentConfig.Variables ?? [];
        var childVariables = child.Variables ?? [];
        var childVariableNames = new HashSet<string>(
            childVariables.Select(x => x.Name ?? ""),
            StringComparer.OrdinalIgnoreCase);

        return childVariables
            .Concat(parentVariables.Where(v => !childVariableNames.Contains(v.Name ?? "")))
            .Select(v => v.Clone())
            .ToArray();
    }
}
