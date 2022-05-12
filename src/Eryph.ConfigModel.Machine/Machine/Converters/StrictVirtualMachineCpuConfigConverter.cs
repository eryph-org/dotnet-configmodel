using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class StrictVirtualMachineCpuConfigConverter : DictionaryConverterBase<VirtualMachineCpuConfig, MachineConfig>
    {
        public override VirtualMachineCpuConfig ConvertFromDictionary(IConverterContext<MachineConfig> context, IDictionary<string, object> dictionary)
        {
            var config = ConvertDictionary(GetValueCaseInvariant(dictionary, nameof(VirtualMachineConfig.Cpu)));

            if (config == null) return null;
            
            return ConvertCpuConfig(config);
        }

        protected virtual VirtualMachineCpuConfig ConvertCpuConfig(object configObject)
        {
            if (configObject is IDictionary<string, object> dictionary)
            {
                return new VirtualMachineCpuConfig
                {
                    Count = GetIntProperty(dictionary, nameof(VirtualMachineCpuConfig.Count))
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}