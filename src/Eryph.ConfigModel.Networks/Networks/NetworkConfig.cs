using JetBrains.Annotations;

namespace Eryph.ConfigModel.Networks
{
    [PublicAPI]
    public class NetworkConfig
    {

        public string? Name { get; set; }
        public string? Environment { get; set; }
        
        [PrivateIdentifier]
        public string? Address { get; set; }
        
        public ProviderConfig? Provider { get; set; }
        
        public NetworkSubnetConfig[]? Subnets { get; set; }
    }
}