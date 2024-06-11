using System.Collections.Generic;
using Eryph.ConfigModel.Catlets.Converters;
using Eryph.ConfigModel.Converters;
using Eryph.ConfigModel.Variables;

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
                new FodderConfigConverter<CatletConfig>(),
                new FodderConfigConverter<CatletConfig>.List(),
                looseMode
                    ? new LooseCatletCapabilitiesConfigConverter()
                    : new StrictCatletCapabilityConfigConverter(),
                new StrictCatletCapabilityConfigConverter.List(),
                new VariableConfigConverter<CatletConfig>(),
                new VariableConfigConverter<CatletConfig>.List(),
                new VariableBindingConfigConverter<CatletConfig>(),
                new VariableBindingConfigConverter<CatletConfig>.List(),
            };

            var context = new ConverterContext<CatletConfig>(
                new DictionaryConverterProvider<CatletConfig>(converters));

            return context.Convert<CatletConfig>(dictionary) ?? new CatletConfig();
        }


    }
}
