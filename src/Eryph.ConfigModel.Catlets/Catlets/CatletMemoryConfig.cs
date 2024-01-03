using System;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]

    public class CatletMemoryConfig : ICloneable
    {
        public int? Startup { get; set; }

        public int? Minimum { get; set; }

        public int? Maximum { get; set; }

        public CatletMemoryConfig Clone()
        {
            return new CatletMemoryConfig
            {
                Startup = Startup,
                Minimum = Minimum,
                Maximum = Maximum
            };
        }
        
        object ICloneable.Clone()
        {
            return Clone();
        }
        
        internal static CatletMemoryConfig? Breed(CatletConfig parentConfig, CatletConfig child)
        {
            if (child.Memory == null)
                return parentConfig.Memory?.Clone();

            var result = child.Memory.Clone();
            result.Startup ??= parentConfig.Memory?.Startup;
            result.Minimum ??= parentConfig.Memory?.Minimum;
            result.Maximum ??= parentConfig.Memory?.Maximum;
            return result;
        }
    }
}