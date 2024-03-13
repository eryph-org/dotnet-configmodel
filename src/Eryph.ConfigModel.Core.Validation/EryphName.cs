using System;
using System.Collections.Generic;
using System.Text;
using Dbosoft.Functional.DataTypes;
using LanguageExt.ClassInstances;

namespace Eryph.ConfigModel;

public abstract class EryphName<NEWTYPE>(string value)
    : ValidatingNewType<NEWTYPE, string, OrdStringOrdinalIgnoreCase>(Normalize(value))
    where NEWTYPE : EryphName<NEWTYPE>
{
    private static string Normalize(string value) => value?.ToLowerInvariant();
}
