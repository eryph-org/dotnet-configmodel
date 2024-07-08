using System;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets;

[PublicAPI]

public class CatletMemoryConfig : ICloneableConfig<CatletMemoryConfig>
{
    public int? Startup { get; set; }

    public int? Minimum { get; set; }

    public int? Maximum { get; set; }

    public CatletMemoryConfig Clone() => new()
    {
        Startup = Startup,
        Minimum = Minimum,
        Maximum = Maximum
    };
}
