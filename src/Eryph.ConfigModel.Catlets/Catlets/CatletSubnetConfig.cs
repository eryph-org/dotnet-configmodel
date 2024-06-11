using System;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletSubnetConfig : ICloneable, ICloneableConfig<CatletSubnetConfig>
    {
        public string? Name { get; set; }
        public string? IpPool { get; set; }

        public CatletSubnetConfig Clone()
        {
            return new CatletSubnetConfig
            {
                Name = Name,
                IpPool = IpPool
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        
    }
}