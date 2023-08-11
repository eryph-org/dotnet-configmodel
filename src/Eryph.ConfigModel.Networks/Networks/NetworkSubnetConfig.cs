using JetBrains.Annotations;

namespace Eryph.ConfigModel.Networks
{
    [PublicAPI]
    public class NetworkSubnetConfig
    {
        public string? Name { get; set; }
        
        [PrivateIdentifier]
        public string? Address { get; set; }
        
        public IpPoolConfig[]? IpPools { get; set; }
        
        public string[]? DnsServers { get; set; }
        public int? Mtu  { get; set; }
    }
}