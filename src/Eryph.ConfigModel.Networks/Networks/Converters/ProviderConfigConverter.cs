using System.Collections.Generic;
using System.Linq;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class StrictProviderConfigConverter : DictionaryConverterBase<ProviderConfig, ProjectNetworksConfig>
    {

        public override ProviderConfig ConvertFromDictionary(IConverterContext<ProjectNetworksConfig> context, 
            IDictionary<object, object> dictionary, object data = null)
        {
            var providerConfig = ConvertDictionary(GetValueCaseInvariant(dictionary, 
                nameof(NetworkConfig.Provider)));

            return providerConfig == null
                ? null
                : ConvertProviderConfig(providerConfig, context);
        }

        protected virtual ProviderConfig ConvertProviderConfig(object configObject, 
            IConverterContext<ProjectNetworksConfig> context)
        {
            if (configObject is IDictionary<object, object> dictionary)
                return new ProviderConfig
                {
                    Name = GetStringProperty(dictionary, nameof(ProviderConfig.Name)),
                    SubnetName = GetStringProperty(dictionary, "subnet", nameof(ProviderConfig.SubnetName)),
                    IpPoolName = GetStringProperty(dictionary, "ipPool", nameof(ProviderConfig.IpPoolName)),
                };
            throw new InvalidConfigModelException();
        }
    }

}