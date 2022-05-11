using System.Collections.Generic;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine.V1
{
    [PublicAPI]
    public class MachineConfig
    {
        public string Name { get; set; }
        public string Environment { get; set; }
        public string Project { get; set; }

        public VirtualMachineConfig VM { get; set; }

        public List<MachineNetworkConfig> Networks { get; set; }

        public MachineProvisioningConfig Provisioning { get; set; }
    }
}