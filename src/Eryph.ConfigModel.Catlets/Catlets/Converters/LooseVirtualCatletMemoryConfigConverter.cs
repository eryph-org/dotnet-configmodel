namespace Eryph.ConfigModel.Catlets.Converters
{
    public class LooseVirtualCatletMemoryConfigConverter : StrictVirtualCatletMemoryConfigConverter
    {
        protected override VirtualCatletMemoryConfig ConvertMemoryConfig(object configObject)
        {
            if (configObject is int number)
            {
                return new VirtualCatletMemoryConfig
                {
                    Startup = number
                };

            }
            return base.ConvertMemoryConfig(configObject);
        }

    }
}