using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine.V1
{
    [PublicAPI]
    public class VirtualMachineNetworkAdapterConfig
    {
        public string Name { get; set; }

        public string MacAddress { get; set; }
    }
}