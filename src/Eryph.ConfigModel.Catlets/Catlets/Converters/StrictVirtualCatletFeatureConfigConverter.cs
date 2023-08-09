using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class StrictVirtualCatletFeatureConfigConverter : DictionaryConverterBase<VirtualCatletCapabilityConfig, CatletConfig>
    {
        public class List : DictionaryToListConverter<VirtualCatletCapabilityConfig[], CatletConfig>
        {
            public List() : base(nameof(VirtualCatletConfig.Capabilities))
            {
            }
        }
        
        public override VirtualCatletCapabilityConfig ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object data = null)
        {
            return ConvertCapabilityConfigConfig(dictionary);
        }
        
        protected virtual VirtualCatletCapabilityConfig ConvertCapabilityConfigConfig(object configObject)
        {
            if (configObject is IDictionary<object, object> dictionary)
            {
                return new VirtualCatletCapabilityConfig
                {
                    Name = GetStringProperty(dictionary, nameof(VirtualCatletCapabilityConfig.Name)),
                    Details = GetListProperty<string>(dictionary, nameof(VirtualCatletCapabilityConfig.Details))
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}