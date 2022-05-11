using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine.V1
{
    [PublicAPI]

    public class VirtualMachineMemoryConfig
    {
        public int? Startup { get; set; }

        public int? Minimum { get; set; }

        public int? Maximum { get; set; }
    }
}