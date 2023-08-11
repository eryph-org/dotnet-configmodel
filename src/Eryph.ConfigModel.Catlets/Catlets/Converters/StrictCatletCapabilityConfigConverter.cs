using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class StrictCatletCapabilityConfigConverter : DictionaryConverterBase<CatletCapabilityConfig, CatletConfig>
    {
        public class List : DictionaryToListConverter<CatletCapabilityConfig[], CatletConfig>
        {
            public List() : base(nameof(CatletConfig.Capabilities))
            {
            }
        }
        
        public override CatletCapabilityConfig? ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object? data = null)
        {
            return ConvertCapabilityConfigConfig(dictionary);
        }
        
        protected virtual CatletCapabilityConfig? ConvertCapabilityConfigConfig(object configObject)
        {
            if (configObject is IDictionary<object, object> dictionary)
            {
                var mutationString = GetStringProperty(dictionary, nameof(CatletCapabilityConfig.Mutation));
                MutationType? mutation = null;
                if (mutationString != null)
                {
                    if (Enum.TryParse(mutationString, true, out MutationType mutationOut))
                        mutation = mutationOut;
                }
                
                return new CatletCapabilityConfig
                {
                    Name = GetStringProperty(dictionary, nameof(CatletCapabilityConfig.Name)),
                    Details = GetListProperty<string>(dictionary, nameof(CatletCapabilityConfig.Details)),
                    Mutation = mutation
                };

            }

            throw new InvalidConfigModelException();

        }

    }
}