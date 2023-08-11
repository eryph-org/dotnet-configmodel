using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Networks.Converters
{
    public class LooseProviderConfigConverter : StrictProviderConfigConverter
    {
        
        protected override ProviderConfig ConvertProviderConfig(object configObject, IConverterContext<ProjectNetworksConfig> context)
        {
            if (configObject is string stringValue)
                return new ProviderConfig { Name = stringValue };
            
            return base.ConvertProviderConfig(configObject, context);
        }
        
    }
}