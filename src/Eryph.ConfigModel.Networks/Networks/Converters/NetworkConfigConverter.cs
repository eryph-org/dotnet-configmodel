using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Networks.Converters
{
    public class NetworkConfigConverter : DictionaryConverterBase<NetworkConfig, ProjectNetworksConfig>
    {
        public class List : DictionaryToListConverter<NetworkConfig[], ProjectNetworksConfig>
        {
            public List() : base(nameof(ProjectNetworksConfig.Networks))
            {
            }
        }

        public override NetworkConfig ConvertFromDictionary(
            IConverterContext<ProjectNetworksConfig> context, IDictionary<object, object> dictionary,
            object? data = default)
        {
            return new NetworkConfig
            {
                Name = GetStringProperty(dictionary, nameof(NetworkConfig.Name)),
                Environment = GetStringProperty(dictionary, nameof(NetworkConfig.Environment)),
                Address = GetStringProperty(dictionary, nameof(NetworkConfig.Address)),
                Subnets = context.ConvertList<NetworkSubnetConfig>(dictionary),
                Provider = context.Convert<ProviderConfig>(dictionary)
            };
        }

    }
}