using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class NetworkSubnetConfig
    {
        public string Name { get; set; }
        public string Address { get; set; }
        
        public IpPoolConfig[] IpPools { get; set; }
        
        public string[] DnsServers { get; set; }
        public int MTU  { get; set; }
    }
}