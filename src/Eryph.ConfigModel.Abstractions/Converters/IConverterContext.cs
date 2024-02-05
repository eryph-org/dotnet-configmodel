using System.Collections.Generic;

namespace Eryph.ConfigModel.Converters
{
    public interface IConverterContext<T>
    {
        T? Target { get; set; }
        
        IDictionaryConverterProvider<T> ConverterProvider { get; }
        TRes? Convert<TRes>(IDictionary<object, object> dictionary, object? data = null) where TRes : class;
        TRes[]? ConvertList<TRes>(IDictionary<object, object> dictionary, object? data = null) where TRes : class;
    }
}