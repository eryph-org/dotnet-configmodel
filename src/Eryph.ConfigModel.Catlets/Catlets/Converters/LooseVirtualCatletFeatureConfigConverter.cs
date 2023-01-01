using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class LooseVirtualCatletFeatureConfigConverter : StrictVirtualCatletFeatureConfigConverter
    {
        public override object ConvertFromObject(IConverterContext<CatletConfig> context, object unConvertedObject, object data = default)
        {
            return unConvertedObject is string 
                ? ConvertFeatureConfig(unConvertedObject) 
                : null;
        }
        
        protected override VirtualCatletFeatureConfig ConvertFeatureConfig(object configObject)
        {
            if (configObject is string name)
                return new VirtualCatletFeatureConfig
                {
                    Name = name
                };
                
            return base.ConvertFeatureConfig(configObject);
        }
        
    }
}