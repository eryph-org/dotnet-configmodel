using System;
using System.Collections.Generic;
using System.Globalization;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class CloudInitConfigConverter : DictionaryConverterBase<FodderConfig, CatletConfig>
    {
        public class List : DictionaryToListConverter<FodderConfig[], CatletConfig>
        {
            public List() : base(nameof(CatletConfig.Fodder))
            {
            }
        }

        public override FodderConfig ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object? data = null)
        {
            return new FodderConfig
            {
                Name = GetStringProperty(dictionary, nameof(FodderConfig.Name)),
                Type = GetStringProperty(dictionary, nameof(FodderConfig.Type)),
                Content = GetStringProperty(dictionary, nameof(FodderConfig.Content)),
                FileName = GetStringProperty(dictionary, nameof(FodderConfig.FileName)),
                Secret = Convert.ToBoolean(GetStringProperty(dictionary, nameof(FodderConfig.Secret)), CultureInfo.InvariantCulture)
            };
        }

    }
}