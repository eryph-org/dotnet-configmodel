using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class StrictCatletMemoryConfigConverter : DictionaryConverterBase<CatletMemoryConfig, CatletConfig>
    {
        public override CatletMemoryConfig? ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object? data = null)
        {
            var config = ConvertDictionary(GetValueCaseInvariant(dictionary, nameof(CatletConfig.Memory)));

            return config == null 
                ? null 
                : ConvertMemoryConfig(config);
        }

        protected virtual CatletMemoryConfig ConvertMemoryConfig(object configObject)
        {
            if (configObject is IDictionary<object, object> dictionary)
            {
                return new CatletMemoryConfig
                {
                    Startup = GetIntProperty(dictionary, nameof(CatletMemoryConfig.Startup)),
                    Minimum = GetIntProperty(dictionary, nameof(CatletMemoryConfig.Minimum)),
                    Maximum = GetIntProperty(dictionary, nameof(CatletMemoryConfig.Maximum))
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}