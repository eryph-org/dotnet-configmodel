using System.Collections.Generic;
using System.Linq;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class NetworkSubnetConfigConverter : DictionaryConverterBase<NetworkSubnetConfig, ProjectNetworksConfig>
    {
        public class List : DictionaryToListConverter<NetworkSubnetConfig[], ProjectNetworksConfig>
        {
            public List() : base(nameof(NetworkConfig.Subnets))
            {
            }
        }
        
        public override NetworkSubnetConfig ConvertFromDictionary(
            IConverterContext<ProjectNetworksConfig> context, IDictionary<object, object> dictionary,
            object data = default)
        {
            return new NetworkSubnetConfig
            {
                Name = GetStringProperty(dictionary, nameof(NetworkSubnetConfig.Name)),
                Address = GetStringProperty(dictionary, nameof(NetworkSubnetConfig.Address)),
                DnsServers = GetListProperty<string>(dictionary, nameof(NetworkSubnetConfig.DnsServers)),
                IpPools = context.ConvertList<IpPoolConfig>(dictionary),
                MTU = GetIntProperty(dictionary,nameof(NetworkSubnetConfig.MTU) )
            };
        }
    }

}