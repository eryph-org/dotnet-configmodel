using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class StrictVirtualMachineMemoryConfigConverter : DictionaryConverterBase<VirtualMachineMemoryConfig, MachineConfig>
    {
        public override VirtualMachineMemoryConfig ConvertFromDictionary(IConverterContext<MachineConfig> context, IDictionary<string, object> dictionary)
        {
            var config = ConvertDictionary(GetValueCaseInvariant(dictionary, nameof(VirtualMachineConfig.Memory)));

            if (config == null) return null;

            return ConvertMemoryConfig(config);
        }

        protected virtual VirtualMachineMemoryConfig ConvertMemoryConfig(object configObject)
        {
            if (configObject is IDictionary<string, object> dictionary)
            {
                return new VirtualMachineMemoryConfig
                {
                    Startup = GetIntProperty(dictionary, nameof(VirtualMachineMemoryConfig.Startup)),
                    Minimum = GetIntProperty(dictionary, nameof(VirtualMachineMemoryConfig.Minimum)),
                    Maximum = GetIntProperty(dictionary, nameof(VirtualMachineMemoryConfig.Maximum))
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}