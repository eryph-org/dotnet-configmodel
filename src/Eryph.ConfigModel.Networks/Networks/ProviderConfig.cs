using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class ProviderConfig
    {

        public string Name { get; set; }
        public string Subnet { get; set; }
        public string IpPool { get; set; }
    }
}