using System.Collections.Generic;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletNetworkConfig
    {

        public string Name { get; set; }
        public string AdapterName { get; set; }
        
        public CatletSubnetConfig SubnetV4 { get; set; }
        public CatletSubnetConfig SubnetV6 { get; set; }
    }
}