using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class StrictVirtualMachineConfigConverter : DictionaryConverterBase<VirtualMachineConfig, MachineConfig>
    {


        public override VirtualMachineConfig ConvertFromDictionary(IConverterContext<MachineConfig> context, IDictionary<string, object> dictionary)
        {
            var vmConfig = ConvertDictionary(GetValueCaseInvariant(dictionary, nameof(MachineConfig.VM)));

            return vmConfig == null
                ? null
                : ConvertVMConfig(vmConfig, context);
        }

        public virtual VirtualMachineConfig ConvertVMConfig(object configObject, IConverterContext<MachineConfig> context)
        {
            if (configObject is IDictionary<string, object> dictionary)
            {
                return new VirtualMachineConfig
                {
                    Image = GetStringProperty(dictionary, nameof(VirtualMachineConfig.Image)),
                    Slug = GetStringProperty(dictionary, nameof(VirtualMachineConfig.Slug)),
                    DataStore = GetStringProperty(dictionary, nameof(VirtualMachineConfig.DataStore)),
                    Cpu = context.Convert<VirtualMachineCpuConfig>(dictionary),
                    Memory = context.Convert<VirtualMachineMemoryConfig>(dictionary),
                    Drives = context.ConvertList<VirtualMachineDriveConfig>(dictionary),
                    NetworkAdapters = context.ConvertList<VirtualMachineNetworkAdapterConfig>(dictionary),
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}
