using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.ConfigModel.Converters
{
    public abstract class DictionaryToListConverter<TConv, TTarget> : DictionaryConverterBase<TConv, TTarget> 
        where TConv : class, IList

    {
        private readonly string[] _listNames;

        protected DictionaryToListConverter(params string[] listNames)
        {
            _listNames = listNames;
        }

        public override TConv ConvertFromDictionary(
            IConverterContext<TTarget> context, IDictionary<object, object> dictionary, 
            object data = null)
        {
            object list = null;
            foreach (var listName in _listNames)
            {
                list = GetValueCaseInvariant(dictionary, listName);
                if (list != null)
                    break;
            }

            if (list == null)
                return default;

            var entryType = CanConvert.GetElementType();

            var listConverterType = typeof(ListConverter<>).MakeGenericType(typeof(TConv), typeof(TTarget), entryType);
            var listConverter = Activator.CreateInstance(listConverterType) as IListConvert;
            var result = listConverter?.ConvertFromDictionary(context, list);
            return result as TConv;
        }

        private interface IListConvert
        {
            object ConvertFromDictionary(IConverterContext<TTarget> context, object list);
        }

        private class ListConverter<T> : IListConvert

        {
            public object ConvertFromDictionary(IConverterContext<TTarget> context, object list)
            {
                switch (list)
                {
                    case null:
                        return default;
                    case IEnumerable enumerable:

                        var result = new List<object>();
                        foreach (var entry in enumerable)
                        {
                            var dictionaryCandidate = ConvertDictionary(entry);
                            if (!(dictionaryCandidate is IDictionary<object, object> entryDictionary)) continue;

                            var convertedEntry = context.ConverterProvider.GetConverter(typeof(T))
                                .ConvertFromDictionary(context, entryDictionary);
                            if (convertedEntry != null)
                                result.Add(convertedEntry);
                        }

                        var res = result.Cast<T>().ToArray();
                        return res;

                }

                return null;
            }
        }
    }
}