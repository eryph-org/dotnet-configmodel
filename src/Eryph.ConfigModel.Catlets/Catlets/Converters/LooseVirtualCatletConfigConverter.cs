using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class LooseVirtualCatletConfigConverter : StrictVirtualCatletConfigConverter
    {

        public override VirtualCatletConfig ConvertVCatletConfig(object vCatletConfigObject, IConverterContext<CatletConfig> context)
        {
            if (vCatletConfigObject is string vCatletImageName)
                return new VirtualCatletConfig { Image = vCatletImageName };

            return base.ConvertVCatletConfig(vCatletConfigObject, context);
        }
    }
}