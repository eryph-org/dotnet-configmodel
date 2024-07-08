using System;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets;

[PublicAPI]
public class CatletCpuConfig : ICloneableConfig<CatletCpuConfig>
{
    public int? Count { get; set; }

    public CatletCpuConfig Clone() => new()
    {
        Count = Count
    };
}
