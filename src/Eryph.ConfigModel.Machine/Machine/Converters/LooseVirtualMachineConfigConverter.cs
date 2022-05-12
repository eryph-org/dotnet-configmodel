using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class LooseVirtualMachineConfigConverter : StrictVirtualMachineConfigConverter
    {

        public override VirtualMachineConfig ConvertVMConfig(object vmConfigObject, IConverterContext<MachineConfig> context)
        {
            if (vmConfigObject is string vmImageName)
                return new VirtualMachineConfig { Image = vmImageName };

            return base.ConvertVMConfig(vmConfigObject, context);
        }
    }
}