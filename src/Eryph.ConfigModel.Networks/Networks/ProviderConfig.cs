using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class ProviderConfig
    {

        public string Name { get; set; }
        public string SubnetName { get; set; }
        public string IpPoolName { get; set; }
    }
}