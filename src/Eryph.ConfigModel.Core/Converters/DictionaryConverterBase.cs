using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.ConfigModel.Converters
{
    public abstract class DictionaryConverterBase<TConv, TTarget> : IDictionaryConverter<TConv,TTarget>
    {
        public abstract TConv? ConvertFromDictionary(IConverterContext<TTarget> context,
            IDictionary<object, object> dictionary, object? data = default);

        public Type CanConvert => typeof(TConv);
        object? IDictionaryConverter<TTarget>.ConvertFromDictionary(
            IConverterContext<TTarget> context, 
            IDictionary<object, object> dictionary, object? data)
        {
            return ConvertFromDictionary(context, dictionary, data);
        }

        public virtual object? ConvertFromObject(IConverterContext<TTarget> context,
            object? unConvertedObject, object? data = default)
        {
            return default;
        }

        protected static object? ConvertDictionary(object? dictionaryCandidate)
        {
            if (dictionaryCandidate is IDictionary<object, object> dictionary)
            {
                return dictionary.ToDictionary(kv => 
                    (object) kv.Key.ToString(), kv => kv.Value);
            }

            return dictionaryCandidate;
        }

        protected static object? GetValueCaseInvariant(IDictionary<object, object> dictionary, params string[] propertyNames)
        {
            var dictionaryKeys = dictionary.Keys.ToArray();
            var lowerCaseKeys = dictionaryKeys
                .Where(k => k is string)
                .Cast<string>()
                .Select(x => x.ToLowerInvariant().Replace("_", "")).ToArray();

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

        protected static string? GetStringProperty(IDictionary<object, object> dictionary, params string[] propertyNames)
        {
            return GetValueCaseInvariant(dictionary, propertyNames)?.ToString().TrimEnd(char.MinValue);
        }
        
        protected static T[]? GetListProperty<T>(IDictionary<object, object> dictionary, 
            params string[] propertyNames)
        {
            if (!(GetValueCaseInvariant(dictionary, propertyNames) is List<object> value)) return null;
            return value.Cast<T>().ToArray();
        }

        protected static int? GetIntProperty(IDictionary<object, object> dictionary, params string[] propertyNames)
        {
            var value = GetStringProperty(dictionary, propertyNames);
            if (value is null)
                return null;

            if(!int.TryParse(value, out var result))
                throw new InvalidConfigModelException($"The value for {propertyNames[0]} is invalid");

            return result;
        }

        protected static bool? GetBoolProperty(IDictionary<object, object> dictionary, params string[] propertyNames)
        {
            var stringValue = GetStringProperty(dictionary, propertyNames);
            if (stringValue is null)
                return null;

            if (!bool.TryParse(stringValue, out var value))
                throw new InvalidConfigModelException($"The value for {propertyNames[0]} is invalid");

            return value;
        }
    }
}
