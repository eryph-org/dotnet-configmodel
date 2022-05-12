using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Machine.Converters
{
    public class VirtualMachineDriveConfigConverter : DictionaryConverterBase<VirtualMachineDriveConfig, MachineConfig>
    {
        public class List: DictionaryToListConverter<VirtualMachineDriveConfig[], MachineConfig>
        {
            public List() : base(nameof(VirtualMachineConfig.Drives))
            {
            }
        }

        public override VirtualMachineDriveConfig ConvertFromDictionary(IConverterContext<MachineConfig> context, IDictionary<string, object> dictionary)
        {
            var typeString = GetStringProperty(dictionary, nameof(VirtualMachineDriveConfig.Type));
            VirtualMachineDriveType? type = null;
            if (typeString != null)
            {
                if (Enum.TryParse(typeString, out VirtualMachineDriveType typeOut))
                    type = typeOut;
            }

            return new VirtualMachineDriveConfig
            {
                Name = GetStringProperty(dictionary, nameof(VirtualMachineDriveConfig.Name)),
                Size = GetIntProperty(dictionary, nameof(VirtualMachineDriveConfig.Size)),
                DataStore = GetStringProperty(dictionary, nameof(VirtualMachineDriveConfig.DataStore)),
                Slug = GetStringProperty(dictionary, nameof(VirtualMachineDriveConfig.Slug)),
                Template = GetStringProperty(dictionary, nameof(VirtualMachineDriveConfig.Template)),
                Type = type
            };
        }
    }
}