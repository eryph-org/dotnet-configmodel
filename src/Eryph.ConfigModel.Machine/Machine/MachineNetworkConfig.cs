using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine
{
    [PublicAPI]
    public class MachineNetworkConfig
    {

        public string Name { get; set; }
        public string AdapterName { get; set; }

        //public List<MachineSubnetConfig> Subnets { get; set; }
    }
}