using System;
using System.Linq;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Variables;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.FodderGenes;

[PublicAPI]
public class FodderGeneConfig : ICloneable, IHasFodderConfig, IHasVariableConfig, ICloneableConfig<FodderGeneConfig>
{
    public string? Version { get; set; }

    public string? Name { get; set; }

    public VariableConfig[]? Variables { get; set; }

    public FodderConfig[]? Fodder { get; set; }

    public FodderGeneConfig Clone()
    {
        return new FodderGeneConfig()
        {
            Version = Version,
            Name = Name,
            Variables = Variables?.Select(x => x.Clone()).ToArray(),
            Fodder = Fodder?.Select(x => x.Clone()).ToArray(),
        };
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}
