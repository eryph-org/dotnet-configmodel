using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine
{
    [PublicAPI]
    public class VirtualMachineNetworkAdapterConfig
    {
        public string Name { get; set; }

        public string MacAddress { get; set; }
    }
}