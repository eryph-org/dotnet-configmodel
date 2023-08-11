using System;
using System.Linq;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletConfig: ICloneable
    {
        public string? Version { get; set; }
        public string? Society { get; set; }
        public string? Name { get; set; }
        public string? Label { get; set; }
        public string? SocialName { get; set; }
        public string? Environment { get; set; }
        public string? Lair { get; set; }
        public string? Parent { get; set; }
        
        public CatletCpuConfig? Cpu { get; set; }

        public CatletMemoryConfig? Memory { get; set; }

        public CatletDriveConfig[]? Drives { get; set; }

        public CatletNetworkAdapterConfig[]? NetworkAdapters { get; set; }
        
        public CatletCapabilityConfig[]? Capabilities  { get; set; }

        public CatletNetworkConfig[]? Networks { get; set; }
        
        public FodderConfig[]? Fodder { get; set; }

        public CatletConfig Breed(CatletConfig child, string? parentReference= default)
        {
            var newConfig = Clone();
            // always only from child
            newConfig.Society = child.Society;
            newConfig.Name = child.Name;
            newConfig.Label = child.Label;
            newConfig.Environment = child.Environment;
            newConfig.SocialName = child.SocialName;
            
            // inheritance from parent
            newConfig.Lair = child.Lair ?? Lair;
            newConfig.Parent = parentReference;
            newConfig.Cpu = CatletCpuConfig.Breed(this, child);
            newConfig.Memory = CatletMemoryConfig.Breed(this, child);
            newConfig.Drives = CatletDriveConfig.Breed(this, child, parentReference);
            newConfig.NetworkAdapters = CatletNetworkAdapterConfig.Breed(this, child);
            newConfig.Capabilities = CatletCapabilityConfig.Breed(this, child);
            newConfig.Networks = CatletNetworkConfig.Breed(this, child);
            newConfig.Fodder = FodderConfig.Breed(this, child, parentReference);

            return newConfig;
        }

        public CatletConfig Clone()
        {
            return new CatletConfig()
            {
                Version = Version,
                Society = Society,
                Name = Name,
                Label = Label,
                SocialName = SocialName,
                Environment = Environment,
                Lair = Lair,
                Parent = Parent,
                Cpu = Cpu?.Clone(),
                Networks = Networks
                    ?.Select(x=>x.Clone()).ToArray(),
                Capabilities = Capabilities?.Select(x=>x.Clone()).ToArray(),
                Drives = Drives?.Select(x=>x.Clone()).ToArray(),
                Fodder = Fodder?.Select(x=>x.Clone()).ToArray(),
                NetworkAdapters = NetworkAdapters?
                    .Select(x=>x.Clone()).ToArray(),
                Memory = Memory?.Clone()
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}