using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class LooseCatletCapabilitiesConfigConverter : StrictCatletCapabilityConfigConverter
    {
        public override object? ConvertFromObject(IConverterContext<CatletConfig> context, 
            object? unConvertedObject, object? data = default)
        {
            return unConvertedObject is string 
                ? ConvertCapabilityConfigConfig(unConvertedObject) 
                : null;
        }
        
        protected override CatletCapabilityConfig? ConvertCapabilityConfigConfig(object configObject)
        {
            if (configObject is string name)
                return new CatletCapabilityConfig
                {
                    Name = name
                };
                
            return base.ConvertCapabilityConfigConfig(configObject);
        }
        
    }
}