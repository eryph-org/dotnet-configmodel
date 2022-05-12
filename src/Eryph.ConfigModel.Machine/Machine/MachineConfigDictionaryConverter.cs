using System.Collections.Generic;
using Eryph.ConfigModel.Converters;
using Eryph.ConfigModel.Machine.Converters;

namespace Eryph.ConfigModel.Machine
{
    public static class MachineConfigDictionaryConverter 
    {

        public static MachineConfig Convert(IDictionary<string, object> dictionary, bool looseMode = false)
        {
            var converters = new IDictionaryConverter<MachineConfig>[]
            {
                new MachineConfigConverter(),
                looseMode
                    ? new LooseVirtualMachineConfigConverter()
                    : new StrictVirtualMachineConfigConverter(),
                looseMode
                    ? new LooseVirtualMachineCpuConfigConverter()
                    : new StrictVirtualMachineCpuConfigConverter(),
                looseMode
                    ? new LooseVirtualMachineMemoryConfigConverter()
                    : new StrictVirtualMachineMemoryConfigConverter(),
                new VirtualMachineDriveConfigConverter(),
                new VirtualMachineDriveConfigConverter.List(),
                new VirtualMachineNetworkAdapterConfigConverter(),
                new VirtualMachineNetworkAdapterConfigConverter.List(),
                new MachineNetworkConfigConverter(),
                new MachineNetworkConfigConverter.List(),
                new MachineProvisioningConfigConverter(),
                new CloudInitConfigConverter(),
                new CloudInitConfigConverter.List()
            };

            var context = new ConverterContext<MachineConfig>(dictionary,
                new DictionaryConverterProvider<MachineConfig>(converters));

            return context.Convert<MachineConfig>(dictionary);
        }


    }
}
