using System;
using System.Collections;
using System.Collections.Generic;

namespace Eryph.ConfigModel.Converters
{
    public class ConverterContext<T> : IConverterContext<T>
    {
        public ConverterContext(IDictionary<object, object> input, IDictionaryConverterProvider<T> converterProvider)
        {
            Input = input;
            ConverterProvider = converterProvider;
        }

        public T Target { get; set; }

        public IDictionary<object, object> Input { get; }

        public IDictionaryConverterProvider<T> ConverterProvider { get;  }

        public TRes Convert<TRes>(IDictionary<object, object> dictionary, object data = null) where TRes : class
        {
            return ConverterProvider.GetConverter(typeof(TRes)).ConvertFromDictionary(this, dictionary, data) as TRes;
        }

        public TRes[] ConvertList<TRes>(IDictionary<object, object> dictionary, object data = null) where TRes : class
        {
            var arrayType = typeof(TRes).MakeArrayType();
            return ConverterProvider.GetConverter(arrayType).ConvertFromDictionary(this, dictionary, data) as TRes[];
        }
    }
    
}