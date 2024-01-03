using System.Collections.Generic;
using Eryph.ConfigModel.Catlets.Converters;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets
{
    public static class CatletConfigDictionaryConverter 
    {

        public static CatletConfig Convert(IDictionary<object, object>? dictionary, bool looseMode = false)
        {
            if (dictionary == null)
                return new CatletConfig();
            
            var converters = new IDictionaryConverter<CatletConfig>[]
            {
                new CatletConfigConverter(),
                looseMode
                    ? new LooseCatletCpuConfigConverter()
                    : new StrictCatletCpuConfigConverter(),
                looseMode
                    ? new LooseCatletMemoryConfigConverter()
                    : new StrictCatletMemoryConfigConverter(),
                new CatletDriveConfigConverter(),
                new CatletDriveConfigConverter.List(),
                new CatletNetworkAdapterConfigConverter(),
                new CatletNetworkAdapterConfigConverter.List(),
                new CatletNetworkConfigConverter(),
                new CatletNetworkConfigConverter.List(),
                new CatletSubnetConfigConverter(),
                new CloudInitConfigConverter(),
                new CloudInitConfigConverter.List(),
                looseMode
                    ? new LooseCatletCapabilitiesConfigConverter()
                    : new StrictCatletCapabilityConfigConverter(),
                new StrictCatletCapabilityConfigConverter.List()
            };

            var context = new ConverterContext<CatletConfig>(dictionary,
                new DictionaryConverterProvider<CatletConfig>(converters));

            return context.Convert<CatletConfig>(dictionary) ?? new CatletConfig();
        }


    }
}
