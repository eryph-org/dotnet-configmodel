using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class StrictVirtualCatletMemoryConfigConverter : DictionaryConverterBase<VirtualCatletMemoryConfig, CatletConfig>
    {
        public override VirtualCatletMemoryConfig ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object data = null)
        {
            var config = ConvertDictionary(GetValueCaseInvariant(dictionary, nameof(VirtualCatletConfig.Memory)));

            if (config == null) return null;

            return ConvertMemoryConfig(config);
        }

        protected virtual VirtualCatletMemoryConfig ConvertMemoryConfig(object configObject)
        {
            if (configObject is IDictionary<object, object> dictionary)
            {
                return new VirtualCatletMemoryConfig
                {
                    Startup = GetIntProperty(dictionary, nameof(VirtualCatletMemoryConfig.Startup)),
                    Minimum = GetIntProperty(dictionary, nameof(VirtualCatletMemoryConfig.Minimum)),
                    Maximum = GetIntProperty(dictionary, nameof(VirtualCatletMemoryConfig.Maximum))
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}