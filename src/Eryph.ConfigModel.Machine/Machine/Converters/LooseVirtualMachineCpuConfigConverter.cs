namespace Eryph.ConfigModel.Machine.Converters
{
    public class LooseVirtualMachineCpuConfigConverter : StrictVirtualMachineCpuConfigConverter
    {
        protected override VirtualMachineCpuConfig ConvertCpuConfig(object configObject)
        {
            if (configObject is int number)
            {
                return new VirtualMachineCpuConfig
                {
                    Count = number
                };

            }

            return base.ConvertCpuConfig(configObject);

        }
    }
}