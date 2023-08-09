using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class VirtualCatletDriveConfig
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Shelter { get; set; }

        [PrivateIdentifier]
        public string Parent { get; set; }

        public int? Size { get; set; }
        public VirtualCatletDriveType? Type { get; set; }
    }
}