using JetBrains.Annotations;

namespace Eryph.ConfigModel.Networks
{
    [PublicAPI]
    public class ProviderConfig
    {

        public string? Name { get; set; }
        public string? Subnet { get; set; }
        public string? IpPool { get; set; }
    }
}