using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public enum VirtualCatletDriveType
    {
        // ReSharper disable InconsistentNaming
        VHD = 0,
        SharedVHD = 1,
        PHD = 2,
        DVD = 3
        // ReSharper restore InconsistentNaming
    }
}