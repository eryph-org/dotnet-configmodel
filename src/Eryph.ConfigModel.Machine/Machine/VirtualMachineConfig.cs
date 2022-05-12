using System.Collections.Generic;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine
{
    [PublicAPI]
    public class VirtualMachineConfig
    {
        public string Slug { get; set; }
        public string DataStore { get; set; }
        public string Image { get; set; }


        public VirtualMachineCpuConfig Cpu { get; set; }

        public VirtualMachineMemoryConfig Memory { get; set; }

        public VirtualMachineDriveConfig[] Drives { get; set; }

        public VirtualMachineNetworkAdapterConfig[] NetworkAdapters { get; set; }
    }
}