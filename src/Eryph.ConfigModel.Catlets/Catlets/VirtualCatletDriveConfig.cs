using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class VirtualCatletDriveConfig
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string DataStore { get; set; }

        public string Template { get; set; }

        public int? Size { get; set; }
        public VirtualCatletDriveType? Type { get; set; }
    }
}