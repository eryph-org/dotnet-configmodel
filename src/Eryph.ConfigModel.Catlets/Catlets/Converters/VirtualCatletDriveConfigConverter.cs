using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class VirtualCatletDriveConfigConverter : DictionaryConverterBase<VirtualCatletDriveConfig, CatletConfig>
    {
        public class List: DictionaryToListConverter<VirtualCatletDriveConfig[], CatletConfig>
        {
            public List() : base(nameof(VirtualCatletConfig.Drives))
            {
            }
        }

        public override VirtualCatletDriveConfig ConvertFromDictionary(
            IConverterContext<CatletConfig> context, IDictionary<object, object> dictionary, object data = null)
        {
            var typeString = GetStringProperty(dictionary, nameof(VirtualCatletDriveConfig.Type));
            VirtualCatletDriveType? type = null;
            if (typeString != null)
            {
                if (Enum.TryParse(typeString, out VirtualCatletDriveType typeOut))
                    type = typeOut;
            }

            return new VirtualCatletDriveConfig
            {
                Name = GetStringProperty(dictionary, nameof(VirtualCatletDriveConfig.Name)),
                Size = GetIntProperty(dictionary, nameof(VirtualCatletDriveConfig.Size)),
                DataStore = GetStringProperty(dictionary, nameof(VirtualCatletDriveConfig.DataStore)),
                Slug = GetStringProperty(dictionary, nameof(VirtualCatletDriveConfig.Slug)),
                Template = GetStringProperty(dictionary, nameof(VirtualCatletDriveConfig.Template)),
                Type = type
            };
        }
    }
}