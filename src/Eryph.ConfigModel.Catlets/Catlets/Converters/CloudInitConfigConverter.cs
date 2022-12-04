using System;
using System.Collections.Generic;
using System.Globalization;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class CloudInitConfigConverter : DictionaryConverterBase<CloudInitConfig, CatletConfig>
    {
        public class List : DictionaryToListConverter<CloudInitConfig[], CatletConfig>
        {
            public List() : base(nameof(CatletRaisingConfig.Config))
            {
            }
        }

        public override CloudInitConfig ConvertFromDictionary(IConverterContext<CatletConfig> context, 
            IDictionary<object, object> dictionary, object data = null)
        {
            return new CloudInitConfig
            {
                Name = GetStringProperty(dictionary, nameof(CloudInitConfig.Name)),
                Type = GetStringProperty(dictionary, nameof(CloudInitConfig.Type)),
                Content = GetStringProperty(dictionary, nameof(CloudInitConfig.Content)),
                FileName = GetStringProperty(dictionary, nameof(CloudInitConfig.FileName)),
                Sensitive = Convert.ToBoolean(GetStringProperty(dictionary, nameof(CloudInitConfig.Sensitive)), CultureInfo.InvariantCulture)
            };
        }

    }
}