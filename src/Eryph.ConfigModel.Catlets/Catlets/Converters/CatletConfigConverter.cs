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
                Project = GetStringProperty(dictionary, nameof(CatletConfig.Project)),
                Hostname = GetStringProperty(dictionary, nameof(CatletConfig.Hostname), "hostname"),
                Parent = GetStringProperty(dictionary, nameof(CatletConfig.Parent)),
                Location = GetStringProperty(dictionary, nameof(CatletConfig.Location)),
                Store = GetStringProperty(dictionary, nameof(CatletConfig.Store)),
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