using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class LooseVirtualCatletFeatureConfigConverter : StrictVirtualCatletFeatureConfigConverter
    {
        public override object ConvertFromObject(IConverterContext<CatletConfig> context, object unConvertedObject, object data = default)
        {
            return unConvertedObject is string 
                ? ConvertCapabilityConfigConfig(unConvertedObject) 
                : null;
        }
        
        protected override VirtualCatletCapabilityConfig ConvertCapabilityConfigConfig(object configObject)
        {
            if (configObject is string name)
                return new VirtualCatletCapabilityConfig
                {
                    Name = name
                };
                
            return base.ConvertCapabilityConfigConfig(configObject);
        }
        
    }
}