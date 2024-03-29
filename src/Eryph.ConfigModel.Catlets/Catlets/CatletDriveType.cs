﻿using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public enum CatletDriveType
    {
        // ReSharper disable InconsistentNaming
        VHD = 0,
        SharedVHD = 1,
        PHD = 2,
        DVD = 3,
        VHDSet = 4,
        // ReSharper restore InconsistentNaming
    }
}