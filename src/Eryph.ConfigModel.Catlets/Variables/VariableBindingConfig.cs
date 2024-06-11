using Eryph.ConfigModel.Catlets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel.Variables;

public class VariableBindingConfig : IMutateableConfig<VariableBindingConfig>
{
    public string? Name { get; set; }
    
    public MutationType? Mutation { get; set; }

    public string? Value { get; set; }

    public bool? Secret { get; set; }

    public VariableBindingConfig Clone() => 
        new()
        {
            Name = Name,
            Mutation = Mutation,
            Value = Value,
            Secret = Secret
        };

    internal static VariableBindingConfig[]? Breed(FodderConfig parentConfig, FodderConfig childConfig)
    {
        return Breeding.WithMutation(parentConfig, childConfig, x => x.Variables,
            (variable, childVariable) =>
            {
                variable.Value = childVariable.Value ?? variable.Value;
                variable.Secret = childVariable.Secret ?? variable.Secret;
            }
        );
    }
}
