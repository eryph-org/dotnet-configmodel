using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class MachineProvisioningConfigConverter : DictionaryConverterBase<MachineProvisioningConfig, MachineConfig>
    {
        public override MachineProvisioningConfig ConvertFromDictionary(IConverterContext<MachineConfig> context,
            IDictionary<string, object> dictionary)
        {
            var config = ConvertDictionary(GetValueCaseInvariant(dictionary, nameof(MachineConfig.Provisioning)));

            return config == null 
                ? null 
                : ConvertConfig(config, context);
        }

        protected virtual MachineProvisioningConfig ConvertConfig(object configObject,
            IConverterContext<MachineConfig> context)
        {
            if (configObject is IDictionary<string, object> dictionary)
            {
                return new MachineProvisioningConfig
                {
                    Hostname = GetStringProperty(dictionary, nameof(MachineProvisioningConfig.Hostname)),
                    Config = context.ConvertList<CloudInitConfig>(dictionary)
                };

            }

            throw new InvalidConfigModelException();

        }
    }
}