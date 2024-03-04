using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.Common;

namespace Eryph.ConfigModel;

public class DataStoreName : EryphName<DataStoreName>
{
    public DataStoreName(string value) : base(value)
    {
        ValidOrThrow(Validations<DataStoreName>.ValidateCharacters(value)
                     | Validations<DataStoreName>.ValidateLength(value, 1, 50));
    }
}
