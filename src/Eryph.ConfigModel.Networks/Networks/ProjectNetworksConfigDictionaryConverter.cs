using System.Collections.Generic;
using Eryph.ConfigModel.Converters;
using Eryph.ConfigModel.Networks.Converters;

namespace Eryph.ConfigModel.Networks
{
    public static class ProjectNetworksConfigDictionaryConverter 
    {

        public static ProjectNetworksConfig Convert(IDictionary<object, object> dictionary, bool looseMode = false)
        {
            var converters = new IDictionaryConverter<ProjectNetworksConfig>[]
            {
                new ProjectNetworksConfigConverter(),
                new NetworkConfigConverter(),
                new NetworkConfigConverter.List(),
                new NetworkSubnetConfigConverter(),
                new NetworkSubnetConfigConverter.List(),
                looseMode 
                    ? new LooseProviderConfigConverter() 
                    :  new StrictProviderConfigConverter(),
                
                new IpPoolConfigConverter(), 
                new IpPoolConfigConverter.List()
            };

            var context = new ConverterContext<ProjectNetworksConfig>(
                new DictionaryConverterProvider<ProjectNetworksConfig>(converters));

            return context.Convert<ProjectNetworksConfig>(dictionary);
        }


    }
}
