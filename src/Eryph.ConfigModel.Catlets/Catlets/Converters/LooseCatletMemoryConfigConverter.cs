namespace Eryph.ConfigModel.Catlets.Converters
{
    public class LooseCatletMemoryConfigConverter : StrictCatletMemoryConfigConverter
    {
        protected override CatletMemoryConfig ConvertMemoryConfig(object configObject)
        {
            if (configObject is int number)
            {
                return new CatletMemoryConfig
                {
                    Startup = number
                };

            }
            return base.ConvertMemoryConfig(configObject);
        }

    }
}