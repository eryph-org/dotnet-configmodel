﻿using System;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletCpuConfig: ICloneable, ICloneableConfig<CatletCpuConfig>
    {
        public int? Count { get; set; }

        public CatletCpuConfig Clone()
        {
            return new CatletCpuConfig
            {
                Count = Count
            };
        }
        object ICloneable.Clone()
        {
            return Clone();
        }

        internal static CatletCpuConfig? Breed(CatletConfig parentConfig, CatletConfig child)
        {
            if (child.Cpu == null)
                return parentConfig.Cpu?.Clone();

            var result = child.Cpu.Clone();
            result.Count ??= parentConfig.Cpu?.Count;
            return result;
        }
    }
}