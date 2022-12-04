using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class CatletRaisingConfigConverter : DictionaryConverterBase<CatletRaisingConfig, CatletConfig>
    {
        public override CatletRaisingConfig ConvertFromDictionary(
            IConverterContext<CatletConfig> context,
            IDictionary<object, object> dictionary, 
            object data = null)
        {
            var config = ConvertDictionary(GetValueCaseInvariant(dictionary,
                nameof(CatletConfig.Raising)));

            return config == null 
                ? null 
                : ConvertConfig(config, context);
        }

        protected virtual CatletRaisingConfig ConvertConfig(object configObject,
            IConverterContext<CatletConfig> context)
        {
            if (configObject is IDictionary<object, object> dictionary)
            {
                return new CatletRaisingConfig
                {
                    Hostname = GetStringProperty(dictionary, nameof(CatletRaisingConfig.Hostname)),
                    Config = context.ConvertList<CloudInitConfig>(dictionary)
                };

            }

            throw new InvalidConfigModelException();

        }
    }
}