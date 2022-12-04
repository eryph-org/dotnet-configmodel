using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class StrictVirtualCatletConfigConverter : DictionaryConverterBase<VirtualCatletConfig, CatletConfig>
    {


        public override VirtualCatletConfig ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object data = null)
        {
            var vCatletConfig = ConvertDictionary(GetValueCaseInvariant(dictionary, "vcatlet"));

            return vCatletConfig == null
                ? null
                : ConvertVCatletConfig(vCatletConfig, context);
        }

        public virtual VirtualCatletConfig ConvertVCatletConfig(object configObject, IConverterContext<CatletConfig> context)
        {
            if (configObject is IDictionary<object, object> dictionary)
            {
                return new VirtualCatletConfig
                {
                    Image = GetStringProperty(dictionary, nameof(VirtualCatletConfig.Image)),
                    Slug = GetStringProperty(dictionary, nameof(VirtualCatletConfig.Slug)),
                    DataStore = GetStringProperty(dictionary, nameof(VirtualCatletConfig.DataStore)),
                    Cpu = context.Convert<VirtualCatletCpuConfig>(dictionary),
                    Memory = context.Convert<VirtualCatletMemoryConfig>(dictionary),
                    Drives = context.ConvertList<VirtualCatletDriveConfig>(dictionary),
                    NetworkAdapters = context.ConvertList<VirtualCatletNetworkAdapterConfig>(dictionary),
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}
