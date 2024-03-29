﻿using System.Collections.Generic;
using Eryph.ConfigModel.Catlets.Converters;
using Eryph.ConfigModel.Converters;

namespace Eryph.ConfigModel.FodderGenes;

public static class FodderGeneConfigDictionaryConverter
{

    public static FodderGeneConfig Convert(IDictionary<object, object>? dictionary, bool looseMode = false)
    {
        if (dictionary == null)
            return new FodderGeneConfig();

        var converters = new IDictionaryConverter<FodderGeneConfig>[]
        {
            new FodderGeneConfigConverter(),
            new FodderConfigConverter<FodderGeneConfig>(),
            new FodderConfigConverter<FodderGeneConfig>.List(),
        };

        var context = new ConverterContext<FodderGeneConfig>(
            new DictionaryConverterProvider<FodderGeneConfig>(converters));

        return context.Convert<FodderGeneConfig>(dictionary) ?? new FodderGeneConfig();
    }


}