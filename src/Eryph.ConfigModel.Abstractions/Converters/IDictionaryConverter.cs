using System;
using System.Collections.Generic;

namespace Eryph.ConfigModel.Converters
{
    public interface IDictionaryConverter<out TConv,TTarget> : IDictionaryConverter<TTarget>
    {
        new TConv? ConvertFromDictionary(IConverterContext<TTarget> context, IDictionary<object, object> dictionary, 
            object? data = default);
        
    }

    public interface IDictionaryConverter<TTarget>
    {
        Type CanConvert { get; }
        object? ConvertFromDictionary(IConverterContext<TTarget> context, IDictionary<object, object> dictionary, object? data = default);

        object? ConvertFromObject(IConverterContext<TTarget> context,
            object? unConvertedObject, object? data = default);
    }
}