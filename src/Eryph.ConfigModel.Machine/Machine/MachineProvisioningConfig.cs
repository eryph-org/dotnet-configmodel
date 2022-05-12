using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine
{
    [PublicAPI]
    public class MachineProvisioningConfig
    {
        public string Hostname { get; set; }

        public CloudInitConfig[] Config { get; set; }
    }

}