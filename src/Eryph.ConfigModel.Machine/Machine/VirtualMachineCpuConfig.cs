using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine
{
    [PublicAPI]
    public class VirtualMachineCpuConfig
    {
        public int? Count { get; set; }
    }
}