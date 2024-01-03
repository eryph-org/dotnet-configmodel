using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class StrictCatletCpuConfigConverter : DictionaryConverterBase<CatletCpuConfig, CatletConfig>
    {
        public override CatletCpuConfig? ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object? data = null)
        {
            var config = ConvertDictionary(GetValueCaseInvariant(dictionary, nameof(CatletConfig.Cpu)));

            if (config == null) return null;
            
            return ConvertCpuConfig(config);
        }

        protected virtual CatletCpuConfig ConvertCpuConfig(object configObject)
        {
            if (configObject is IDictionary<object, object> dictionary)
            {
                return new CatletCpuConfig
                {
                    Count = GetIntProperty(dictionary, nameof(CatletCpuConfig.Count))
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}