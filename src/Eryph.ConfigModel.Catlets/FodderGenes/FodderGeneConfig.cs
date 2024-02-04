using System;
using System.Linq;
using Eryph.ConfigModel.Catlets;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.FodderGenes;

[PublicAPI]
public class FodderGeneConfig : ICloneable, IHasFodderConfig
{
    public string? Version { get; set; }
    public string? Name { get; set; }

    public FodderConfig[]? Fodder { get; set; }

    public FodderGeneConfig Clone()
    {
        return new FodderGeneConfig()
        {
            Version = Version,
            Name = Name,
            Fodder = Fodder?.Select(x => x.Clone()).ToArray(),
        };
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}