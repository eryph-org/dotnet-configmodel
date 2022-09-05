using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.ConfigModel.Converters
{
    public abstract class DictionaryConverterBase<TConv, TTarget> : IDictionaryConverter<TConv,TTarget>
    {
        public abstract TConv ConvertFromDictionary(IConverterContext<TTarget> context,
            IDictionary<string, object> dictionary);

        public Type CanConvert => typeof(TConv);
        object IDictionaryConverter<TTarget>.ConvertFromDictionary(IConverterContext<TTarget> context, IDictionary<string, object> dictionary)
        {
            return ConvertFromDictionary(context, dictionary);
        }

        protected static object ConvertDictionary(object dictionaryCandidate)
        {
            if (dictionaryCandidate is IDictionary<object, object> dictionary)
            {
                return dictionary.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value);
            }

            return dictionaryCandidate;
        }
        
        protected static object GetValueCaseInvariant(IDictionary<string, object> dictionary, params string[] propertyNames)
        {
            var dictionaryKeys = dictionary.Keys.ToArray();
            var lowerCaseKeys = dictionaryKeys.Select(x => x.ToLowerInvariant()).ToArray();

            foreach (var propertyName in propertyNames)
            {
                var lowerCasePropertyName = propertyName.ToLowerInvariant();

                var keyPos = Array.IndexOf(lowerCaseKeys, lowerCasePropertyName);
                if (keyPos == -1)
                    continue;

                var originalKeyName = dictionaryKeys.GetValue(keyPos).ToString();
                return dictionary[originalKeyName];
            }

            return null;
        }

        protected static string GetStringProperty(IDictionary<string, object> dictionary, params string[] propertyNames)
        {
            return GetValueCaseInvariant(dictionary, propertyNames)?.ToString().TrimEnd(char.MinValue);
        }

        protected static int GetIntProperty(IDictionary<string, object> dictionary, params string[] propertyNames)
        {
            return Convert.ToInt32(GetValueCaseInvariant(dictionary, propertyNames));
        }
    }
}