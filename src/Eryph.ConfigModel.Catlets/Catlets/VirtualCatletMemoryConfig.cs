using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]

    public class VirtualCatletMemoryConfig
    {
        public int? Startup { get; set; }

        public int? Minimum { get; set; }

        public int? Maximum { get; set; }
    }
}