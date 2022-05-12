using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine
{
    [PublicAPI]
    public enum VirtualMachineDriveType
    {
        // ReSharper disable InconsistentNaming
        VHD = 0,
        SharedVHD = 1,
        PHD = 2,

        DVD = 3
        // ReSharper restore InconsistentNaming
    }
}