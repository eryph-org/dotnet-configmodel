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
            private readonly Type _type;

            public PlaceHolderConverter(Type type)
            {
                _type = type;
            }

            public Type CanConvert => _type;
            public object ConvertFromDictionary(IConverterContext<TTarget> context, IDictionary<object, object> dictionary, 
                object data= null)
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