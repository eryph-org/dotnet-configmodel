using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters;

public class CatletDriveConfigConverter : DictionaryConverterBase<CatletDriveConfig, CatletConfig>
{
    public class List() : DictionaryToListConverter<CatletDriveConfig[], CatletConfig>(nameof(CatletConfig.Drives));

    public override CatletDriveConfig ConvertFromDictionary(
        IConverterContext<CatletConfig> context,
        IDictionary<object, object> dictionary,
        object? data = null)
    {
        return new CatletDriveConfig
        {
            Name = GetStringProperty(dictionary, nameof(CatletDriveConfig.Name)),
            Size = GetIntProperty(dictionary, nameof(CatletDriveConfig.Size)),
            Store = GetStringProperty(dictionary, nameof(CatletDriveConfig.Store)),
            Location = GetStringProperty(dictionary, nameof(CatletDriveConfig.Location)),
            Source = GetStringProperty(dictionary, nameof(CatletDriveConfig.Source)),
            Mutation = GetEnumProperty<MutationType>(dictionary, nameof(CatletDriveConfig.Mutation)),
            Type = GetEnumProperty<CatletDriveType>(dictionary, nameof(CatletDriveConfig.Type)),
        };
    }
}
