using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class CatletConfigConverter : DictionaryConverterBase<CatletConfig, CatletConfig>
    {

        public override CatletConfig ConvertFromDictionary(
            IConverterContext<CatletConfig> context, IDictionary<object, object> dictionary, object? data = null)
        {
            
            // ReSharper disable once UseObjectOrCollectionInitializer
            // target should be initialized
            context.Target = new CatletConfig
            {
                Name = GetStringProperty(dictionary, nameof(CatletConfig.Name)),
                Environment = GetStringProperty(dictionary, nameof(CatletConfig.Environment)),
                Society = GetStringProperty(dictionary, nameof(CatletConfig.Society)),
                SocialName = GetStringProperty(dictionary, nameof(CatletConfig.SocialName), "hostname"),
                Parent = GetStringProperty(dictionary, nameof(CatletConfig.Parent)),
                Label = GetStringProperty(dictionary, nameof(CatletConfig.Label)),
                Lair = GetStringProperty(dictionary, nameof(CatletConfig.Lair)),
            };

            context.Target.Cpu = context.Convert<CatletCpuConfig>(dictionary);
            context.Target.Memory = context.Convert<CatletMemoryConfig>(dictionary);
            context.Target.Drives = context.ConvertList<CatletDriveConfig>(dictionary);
            context.Target.NetworkAdapters = context.ConvertList<CatletNetworkAdapterConfig>(dictionary);
            context.Target.Capabilities = context.ConvertList<CatletCapabilityConfig>(dictionary);
            context.Target.Networks = context.ConvertList<CatletNetworkConfig>(dictionary);
            context.Target.Fodder = context.ConvertList<FodderConfig>(dictionary);

            return context.Target;
        }



    }
}