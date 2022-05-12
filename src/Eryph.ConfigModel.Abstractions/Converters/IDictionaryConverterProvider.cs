using System;

namespace Eryph.ConfigModel.Converters
{
    public interface IDictionaryConverterProvider<TTarget>
    {
        IDictionaryConverter<TTarget> GetConverter(Type type);
    }
}