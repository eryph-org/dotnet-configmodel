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
}
