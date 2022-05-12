using System;
using System.Collections.Generic;

namespace Eryph.ConfigModel.Converters
{
    public class ConverterContext<T> : IConverterContext<T>
    {
        public ConverterContext(IDictionary<string, object> input, IDictionaryConverterProvider<T> converterProvider)
        {
            Input = input;
            ConverterProvider = converterProvider;
        }

        public T Target { get; set; }

        public IDictionary<string, object> Input { get; }

        public IDictionaryConverterProvider<T> ConverterProvider { get;  }

        public TRes Convert<TRes>(IDictionary<string, object> dictionary) where TRes : class
        {
            return ConverterProvider.GetConverter(typeof(TRes)).ConvertFromDictionary(this, dictionary) as TRes;
        }

        public TRes[] ConvertList<TRes>(IDictionary<string, object> dictionary) where TRes : class
        {
            var arrayType = typeof(TRes).MakeArrayType();
            return ConverterProvider.GetConverter(arrayType).ConvertFromDictionary(this, dictionary) as TRes[];
        }
    }
}