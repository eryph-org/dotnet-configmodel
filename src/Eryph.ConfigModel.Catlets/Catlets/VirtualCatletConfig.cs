using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class VirtualCatletConfig
    {
        public string Slug { get; set; }
        public string Shelter { get; set; }
        public string Parent { get; set; }
        
        public VirtualCatletCpuConfig Cpu { get; set; }

        public VirtualCatletMemoryConfig Memory { get; set; }

        public VirtualCatletDriveConfig[] Drives { get; set; }

        public VirtualCatletNetworkAdapterConfig[] NetworkAdapters { get; set; }
        
        public VirtualCatletCapabilityConfig[] Capabilities  { get; set; }
    }
}