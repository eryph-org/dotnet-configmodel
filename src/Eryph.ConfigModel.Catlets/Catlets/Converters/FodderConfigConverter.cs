using System;
using System.Collections.Generic;
using System.Globalization;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.Catlets.Converters
{
    public class FodderConfigConverter<TConfig> : DictionaryConverterBase<FodderConfig, TConfig>
       where TConfig : IHasFodderConfig
    {
        public class List : DictionaryToListConverter<FodderConfig[], TConfig>
        {
            public List() : base(nameof(IHasFodderConfig.Fodder))
            {
            }
        }

        public override FodderConfig ConvertFromDictionary(IConverterContext<TConfig> context, 
            IDictionary<object, object> dictionary, object? data = null)
        {
            var res = new FodderConfig
            {
                Name = GetStringProperty(dictionary, nameof(FodderConfig.Name)),
                Source = GetStringProperty(dictionary, nameof(FodderConfig.Source)),
                Type = GetStringProperty(dictionary, nameof(FodderConfig.Type)),
                Content = GetStringProperty(dictionary, nameof(FodderConfig.Content)),
                FileName = GetStringProperty(dictionary, nameof(FodderConfig.FileName))
            };

            var secretString = GetStringProperty(dictionary, nameof(FodderConfig.Secret));
            if(!string.IsNullOrEmpty(secretString))
                res.Secret = bool.Parse(secretString);

            var removeString = GetStringProperty(dictionary, nameof(FodderConfig.Remove));
            if (!string.IsNullOrEmpty(removeString))
                res.Remove = bool.Parse(removeString);


            return res;
        }



    }
}