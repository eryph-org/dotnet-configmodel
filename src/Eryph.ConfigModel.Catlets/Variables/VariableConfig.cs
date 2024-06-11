using System;
using System.Collections.Generic;
using System.Text;
using Eryph.ConfigModel.Catlets;

namespace Eryph.ConfigModel.Variables;

public class VariableConfig : ICloneable, IMutateableConfig<VariableConfig>
{
    public string? Name { get; set; }

    public MutationType? Mutation { get; }

    public VariableType? Type { get; set; }

    public string? Value { get; set; }

    public bool? Secret { get; set; }

    public bool? Required { get; set; }
    
    public VariableConfig Clone()
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

    internal static VariableConfig[]? Breed(CatletConfig parentConfig, CatletConfig child)
    {
        return Breeding.WithMutation(parentConfig, child, x => x.Variables,
            (variable, childVariable) =>
            {
                variable.Type = childVariable.Type ?? variable.Type;
                variable.Value = childVariable.Value ?? variable.Value;
                variable.Secret = childVariable.Secret ?? variable.Secret;
                variable.Required = childVariable.Required ?? variable.Required;
            }
        );
    }
}
