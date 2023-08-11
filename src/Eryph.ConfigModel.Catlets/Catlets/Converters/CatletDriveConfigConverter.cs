using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class CatletDriveConfigConverter : DictionaryConverterBase<CatletDriveConfig, CatletConfig>
    {
        public class List: DictionaryToListConverter<CatletDriveConfig[], CatletConfig>
        {
            public List() : base(nameof(CatletConfig.Drives))
            {
            }
        }

        public override CatletDriveConfig ConvertFromDictionary(
            IConverterContext<CatletConfig> context, IDictionary<object, object> dictionary, object? data = null)
        {
            var typeString = GetStringProperty(dictionary, nameof(CatletDriveConfig.Type));
            CatletDriveType? type = null;
            if (typeString != null)
            {
                if (Enum.TryParse(typeString, true, out CatletDriveType typeOut))
                    type = typeOut;
            }
            
            var mutationString = GetStringProperty(dictionary, nameof(CatletDriveConfig.Mutation));
            MutationType? mutation = null;
            if (mutationString != null)
            {
                if (Enum.TryParse(mutationString, true, out MutationType mutationOut))
                    mutation = mutationOut;
            }

            return new CatletDriveConfig
            {
                Name = GetStringProperty(dictionary, nameof(CatletDriveConfig.Name)),
                Size = GetIntProperty(dictionary, nameof(CatletDriveConfig.Size)),
                Lair = GetStringProperty(dictionary, nameof(CatletDriveConfig.Lair)),
                Label = GetStringProperty(dictionary, nameof(CatletDriveConfig.Label)),
                Parent = GetStringProperty(dictionary, nameof(CatletDriveConfig.Parent)),
                Mutation = mutation,
                Type = type
            };
        }
    }
}