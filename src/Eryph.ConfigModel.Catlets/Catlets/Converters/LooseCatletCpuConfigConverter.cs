using System;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class LooseCatletCpuConfigConverter : StrictCatletCpuConfigConverter
    {
        protected override CatletCpuConfig ConvertCpuConfig(object configObject)
        {
            if (configObject is string numberString && int.TryParse(numberString, out var number) )
            {
                return new CatletCpuConfig
                {
                    Count = number
                };

            }

            return base.ConvertCpuConfig(configObject);

        }
    }
}