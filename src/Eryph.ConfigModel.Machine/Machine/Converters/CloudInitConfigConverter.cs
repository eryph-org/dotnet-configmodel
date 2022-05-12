using System;
using System.Collections.Generic;
using System.Globalization;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class CloudInitConfigConverter : DictionaryConverterBase<CloudInitConfig, MachineConfig>
    {
        public class List : DictionaryToListConverter<CloudInitConfig[], MachineConfig>
        {
            public List() : base(nameof(MachineProvisioningConfig.Config))
            {
            }
        }

        public override CloudInitConfig ConvertFromDictionary(IConverterContext<MachineConfig> context, IDictionary<string, object> dictionary)
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