using System.Collections.Generic;
using System.Linq;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class IpPoolConfigConverter : DictionaryConverterBase<IpPoolConfig, ProjectNetworksConfig>
    {
        public class List : DictionaryToListConverter<IpPoolConfig[], ProjectNetworksConfig>
        {
            public List() : base(nameof(NetworkSubnetConfig.IpPools))
            {
            }
        }
        
        public override IpPoolConfig ConvertFromDictionary(
            IConverterContext<ProjectNetworksConfig> context, IDictionary<object, object> dictionary,
            object data = default)
        {
            return new IpPoolConfig
            {
                Name = GetStringProperty(dictionary, nameof(IpPoolConfig.Name)),
                FirstIp = GetStringProperty(dictionary, nameof(IpPoolConfig.FirstIp)),
                LastIp = GetStringProperty(dictionary, nameof(IpPoolConfig.LastIp))
            };
        }
    }

}