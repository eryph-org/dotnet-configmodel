using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class VirtualMachineNetworkAdapterConfigConverter : DictionaryConverterBase<VirtualMachineNetworkAdapterConfig, MachineConfig>
    {
        public class List : DictionaryToListConverter<VirtualMachineNetworkAdapterConfig[], MachineConfig>
        {
            public List() : base(nameof(VirtualMachineConfig.NetworkAdapters), "network_adapters")
            {
            }
        }
        public override VirtualMachineNetworkAdapterConfig ConvertFromDictionary(IConverterContext<MachineConfig> context, IDictionary<string, object> dictionary)
        {
            return new VirtualMachineNetworkAdapterConfig
            {
                Name = GetStringProperty(dictionary, nameof(VirtualMachineNetworkAdapterConfig.Name)),
                MacAddress = GetStringProperty(dictionary,
                    nameof(VirtualMachineNetworkAdapterConfig.MacAddress),
                    "mac_address"),
            };
        }
    }
}