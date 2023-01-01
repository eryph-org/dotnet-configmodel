using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class StrictVirtualCatletFeatureConfigConverter : DictionaryConverterBase<VirtualCatletFeatureConfig, CatletConfig>
    {
        public class List : DictionaryToListConverter<VirtualCatletFeatureConfig[], CatletConfig>
        {
            public List() : base(nameof(VirtualCatletConfig.Features))
            {
            }
        }
        
        public override VirtualCatletFeatureConfig ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object data = null)
        {
            return ConvertFeatureConfig(dictionary);
        }
        
        protected virtual VirtualCatletFeatureConfig ConvertFeatureConfig(object configObject)
        {
            if (configObject is IDictionary<object, object> dictionary)
            {
                return new VirtualCatletFeatureConfig
                {
                    Name = GetStringProperty(dictionary, nameof(VirtualCatletFeatureConfig.Name)),
                    Settings = GetListProperty<string>(dictionary, nameof(VirtualCatletFeatureConfig.Settings))
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}