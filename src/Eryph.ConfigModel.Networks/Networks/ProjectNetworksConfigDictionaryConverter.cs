using System.Collections.Generic;
using Eryph.ConfigModel.Catlets.Converters;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets
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

            var context = new ConverterContext<ProjectNetworksConfig>(dictionary,
                new DictionaryConverterProvider<ProjectNetworksConfig>(converters));

            return context.Convert<ProjectNetworksConfig>(dictionary);
        }


    }
}
