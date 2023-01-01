using System.Collections.Generic;
using Eryph.ConfigModel.Catlets.Converters;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets
{
    public static class CatletConfigDictionaryConverter 
    {

        public static CatletConfig Convert(IDictionary<object, object> dictionary, bool looseMode = false)
        {
            var converters = new IDictionaryConverter<CatletConfig>[]
            {
                new CatletConfigConverter(),
                looseMode
                    ? new LooseVirtualCatletConfigConverter()
                    : new StrictVirtualCatletConfigConverter(),
                looseMode
                    ? new LooseVirtualCatletCpuConfigConverter()
                    : new StrictVirtualCatletCpuConfigConverter(),
                looseMode
                    ? new LooseVirtualCatletMemoryConfigConverter()
                    : new StrictVirtualCatletMemoryConfigConverter(),
                new VirtualCatletDriveConfigConverter(),
                new VirtualCatletDriveConfigConverter.List(),
                new VirtualCatletNetworkAdapterConfigConverter(),
                new VirtualCatletNetworkAdapterConfigConverter.List(),
                new CatletNetworkConfigConverter(),
                new CatletNetworkConfigConverter.List(),
                new CatletSubnetConfigConverter(),
                new CatletRaisingConfigConverter(),
                new CloudInitConfigConverter(),
                new CloudInitConfigConverter.List(),
                looseMode
                    ? new LooseVirtualCatletFeatureConfigConverter()
                    : new StrictVirtualCatletFeatureConfigConverter(),
                new StrictVirtualCatletFeatureConfigConverter.List()
            };

            var context = new ConverterContext<CatletConfig>(dictionary,
                new DictionaryConverterProvider<CatletConfig>(converters));

            return context.Convert<CatletConfig>(dictionary);
        }


    }
}
