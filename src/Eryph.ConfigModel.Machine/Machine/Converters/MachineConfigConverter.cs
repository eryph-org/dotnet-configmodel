using System.Collections.Generic;
using System.Reflection;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class MachineConfigConverter : DictionaryConverterBase<MachineConfig, MachineConfig>
    {

        public override MachineConfig ConvertFromDictionary(IConverterContext<MachineConfig> context, IDictionary<string, object> dictionary)
        {
            
            // ReSharper disable once UseObjectOrCollectionInitializer
            // target should be initialized
            context.Target = new MachineConfig
            {
                Name = GetStringProperty(dictionary, nameof(MachineConfig.Name)),
                Environment = GetStringProperty(dictionary, nameof(MachineConfig.Environment)),
                Project = GetStringProperty(dictionary, nameof(MachineConfig.Project)),
            };

            context.Target.VM = context.Convert<VirtualMachineConfig>(dictionary);
            context.Target.Networks = context.ConvertList<MachineNetworkConfig>(dictionary);
            context.Target.Provisioning = context.Convert<MachineProvisioningConfig>(dictionary);

            return context.Target;
        }



    }
}