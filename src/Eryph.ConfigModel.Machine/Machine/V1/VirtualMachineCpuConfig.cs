using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine.V1
{
    [PublicAPI]
    public class VirtualMachineCpuConfig
    {
        public int? Count { get; set; }
    }
}