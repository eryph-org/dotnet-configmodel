using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.ClassInstances.Const;
using LanguageExt.ClassInstances.Pred;
using LanguageExt.Common;
using LanguageExt.TypeClasses;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
    public abstract class EryphName<NEWTYPE>(string value)
        : ValidatingNewType<NEWTYPE, string, OrdStringOrdinalIgnoreCase>(value)
        where NEWTYPE : EryphName<NEWTYPE>;
        //where VALIDATING : struct, Validating<string>;

    public struct IsErpyhName : PredWithMessage<string>
    {
        public bool True(string value) =>
            Optional(value)
                .Filter(v => v.Length is >= 1 and <= 50)
                .Filter(v => Regex.IsMatch(v, @"^[a-z0-9]+([\-\.][a-z0-9]+)*$", RegexOptions.Compiled, TimeSpan.FromSeconds(3)))
                .IsSome;

        public string Message =>
            "The name must be between 1 and 50 alphanumeric characters. Dashes and dot are allowed but not at the beginning or end.";
    }
}
