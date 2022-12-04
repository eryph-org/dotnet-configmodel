using System.Collections.Generic;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class NetworkConfig
    {

        public string Name { get; set; }
        public string Environment { get; set; }
        public string Address { get; set; }
        
        public ProviderConfig Provider { get; set; }
        
        public NetworkSubnetConfig[] Subnets { get; set; }
    }
}