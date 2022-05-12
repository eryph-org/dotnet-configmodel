using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class MachineNetworkConfigConverter : DictionaryConverterBase<MachineNetworkConfig, MachineConfig>
    {
        public class List : DictionaryToListConverter<MachineNetworkConfig[], MachineConfig>
        {
            public List() : base(nameof(MachineConfig.Networks))
            {
            }
        }

        public override MachineNetworkConfig ConvertFromDictionary(IConverterContext<MachineConfig> context, IDictionary<string, object> dictionary)
        {
            return new MachineNetworkConfig
            {
                Name = GetStringProperty(dictionary, nameof(MachineNetworkConfig.Name)),
                AdapterName = GetStringProperty(dictionary, nameof(MachineNetworkConfig.AdapterName), "adapter_name"),
            };
        }

    }
}