using JetBrains.Annotations;

namespace Eryph.ConfigModel.Networks
{
    [PublicAPI]
    public class ProjectNetworksConfig
    {
        public string? Version { get; set; }
        public string? Project { get; set; }

        public NetworkConfig[]? Networks { get; set; }
    }
}