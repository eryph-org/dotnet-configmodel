namespace Eryph.ConfigModel.Machine.Converters
{
    public class LooseVirtualMachineMemoryConfigConverter : StrictVirtualMachineMemoryConfigConverter
    {
        protected override VirtualMachineMemoryConfig ConvertMemoryConfig(object configObject)
        {
            if (configObject is int number)
            {
                return new VirtualMachineMemoryConfig
                {
                    Startup = number
                };

            }
            return base.ConvertMemoryConfig(configObject);
        }

    }
}