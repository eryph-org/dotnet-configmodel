using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class CatletConfigConverter : DictionaryConverterBase<CatletConfig, CatletConfig>
    {

        public override CatletConfig ConvertFromDictionary(
            IConverterContext<CatletConfig> context, IDictionary<object, object> dictionary, object data = null)
        {
            
            // ReSharper disable once UseObjectOrCollectionInitializer
            // target should be initialized
            context.Target = new CatletConfig
            {
                Name = GetStringProperty(dictionary, nameof(CatletConfig.Name)),
                Environment = GetStringProperty(dictionary, nameof(CatletConfig.Environment)),
                Project = GetStringProperty(dictionary, nameof(CatletConfig.Project)),
            };

            context.Target.VCatlet = context.Convert<VirtualCatletConfig>(dictionary);
            context.Target.Networks = context.ConvertList<CatletNetworkConfig>(dictionary);
            context.Target.Raising = context.Convert<CatletRaisingConfig>(dictionary);

            return context.Target;
        }



    }
}