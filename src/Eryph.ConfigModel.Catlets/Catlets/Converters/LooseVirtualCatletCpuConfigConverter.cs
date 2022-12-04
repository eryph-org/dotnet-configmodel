using System;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class LooseVirtualCatletCpuConfigConverter : StrictVirtualCatletCpuConfigConverter
    {
        protected override VirtualCatletCpuConfig ConvertCpuConfig(object configObject)
        {
            if (configObject is string numberString && int.TryParse(numberString, out var number) )
            {
                return new VirtualCatletCpuConfig
                {
                    Count = number
                };

            }

            return base.ConvertCpuConfig(configObject);

        }
    }
}