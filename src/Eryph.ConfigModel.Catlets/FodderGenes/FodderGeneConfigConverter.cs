using System.Collections.Generic;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Converters;
using Eryph.ConfigModel.Variables;

namespace Eryph.ConfigModel.FodderGenes;

public class FodderGeneConfigConverter : DictionaryConverterBase<FodderGeneConfig, FodderGeneConfig>
{

    public override FodderGeneConfig ConvertFromDictionary(
        IConverterContext<FodderGeneConfig> context, IDictionary<object, object> dictionary, object? data = null)
    {

        // ReSharper disable once UseObjectOrCollectionInitializer
        // target should be initialized
        context.Target = new FodderGeneConfig
        {
            Version = GetStringProperty(dictionary, nameof(FodderGeneConfig.Version)),
            Name = GetStringProperty(dictionary, nameof(FodderGeneConfig.Name)),
        };

        context.Target.Fodder = context.ConvertList<FodderConfig>(dictionary);
        context.Target.Variables = context.ConvertList<VariableConfig>(dictionary);

        return context.Target;
    }

}