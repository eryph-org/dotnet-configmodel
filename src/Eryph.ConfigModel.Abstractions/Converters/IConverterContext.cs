using System.Collections.Generic;

namespace Eryph.ConfigModel.Converters
{
    public interface IConverterContext<T>
    {
        T Target { get; set; }
        IDictionary<string, object> Input { get; }
        IDictionaryConverterProvider<T> ConverterProvider { get; }
        TRes Convert<TRes>(IDictionary<string, object> dictionary) where TRes : class;
        TRes[] ConvertList<TRes>(IDictionary<string, object> dictionary) where TRes : class;
    }
}