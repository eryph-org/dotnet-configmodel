using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryph.ConfigModel.Catlets;

namespace Eryph.ConfigModel.Variables;

public class VariableConfig : ICloneable, ICloneableConfig<VariableConfig>
{
    public string? Name { get; set; }

    public VariableType? Type { get; set; }

    public string? Value { get; set; }

    public bool? Secret { get; set; }

    public bool? Required { get; set; }
    
    public new VariableConfig Clone()
    {
        return new VariableConfig
        {
            Name = Name,
            Type = Type,
            Value = Value,
            Secret = Secret,
            Required = Required,
        };
    }

    object ICloneable.Clone()
    {
        return Clone();
    }

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
