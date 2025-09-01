using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public enum CatletDriveType
    {
        // ReSharper disable InconsistentNaming
        Vhd = 0,
        SharedVhd = 1,
        Phd = 2,
        Dvd = 3,
        VhdSet = 4,
        // ReSharper restore InconsistentNaming
    }
}