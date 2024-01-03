using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Eryph.ConfigModel.Converters
{
    public class DictionaryConverterProvider<TTarget> : IDictionaryConverterProvider<TTarget>
    {
        private readonly IDictionary<Type, IDictionaryConverter<TTarget>> _converters =
            new Dictionary<Type, IDictionaryConverter<TTarget>>();

        public DictionaryConverterProvider(IEnumerable<IDictionaryConverter<TTarget>> converters)
        {
            AddConverters(converters);
        }

        public void AddConverters(IEnumerable<IDictionaryConverter<TTarget>> converters)
        {
            foreach (var converter in converters)
            {
                _converters.Add(converter.CanConvert, converter);
            }
        }
        
        [ExcludeFromCodeCoverage]
        private class PlaceHolderConverter : IDictionaryConverter<TTarget>
        {
            public PlaceHolderConverter(Type type)
            {
                CanConvert = type;
            }

            public Type CanConvert { get; }

            public object? ConvertFromDictionary(IConverterContext<TTarget> context, IDictionary<object, object> dictionary, 
                object? data= null)
            {
                return default;
            }

            public object? ConvertFromObject(IConverterContext<TTarget> context, object? unConvertedObject, object? data = default)
            {
                return default;
            }
        }

        public IDictionaryConverter<TTarget> GetConverter(Type type)
        {
            if (!_converters.ContainsKey(type))
            {
                return new PlaceHolderConverter(type);
            }

            return _converters[type];
        }
    }
}