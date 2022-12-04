using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class StrictVirtualCatletCpuConfigConverter : DictionaryConverterBase<VirtualCatletCpuConfig, CatletConfig>
    {
        public override VirtualCatletCpuConfig ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object data = null)
        {
            var config = ConvertDictionary(GetValueCaseInvariant(dictionary, nameof(VirtualCatletConfig.Cpu)));

            if (config == null) return null;
            
            return ConvertCpuConfig(config);
        }

        protected virtual VirtualCatletCpuConfig ConvertCpuConfig(object configObject)
        {
            if (configObject is IDictionary<object, object> dictionary)
            {
                return new VirtualCatletCpuConfig
                {
                    Count = GetIntProperty(dictionary, nameof(VirtualCatletCpuConfig.Count))
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}