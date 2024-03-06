using System;
using System.Collections.Generic;
using System.Text;
using Dbosoft.Functional.DataTypes;
using LanguageExt.ClassInstances;

namespace Eryph.ConfigModel;

public abstract class EryphName<NEWTYPE>(string value)
    : ValidatingNewType<NEWTYPE, string, OrdStringOrdinalIgnoreCase>(value)
    where NEWTYPE : EryphName<NEWTYPE>;
