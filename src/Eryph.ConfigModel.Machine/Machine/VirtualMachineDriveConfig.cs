using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine
{
    [PublicAPI]
    public class VirtualMachineDriveConfig
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string DataStore { get; set; }

        public string Template { get; set; }

        public int? Size { get; set; }
        public VirtualMachineDriveType? Type { get; set; }
    }
}