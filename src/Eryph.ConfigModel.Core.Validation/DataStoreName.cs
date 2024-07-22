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
        ValidOrThrow(Validations<DataStoreName>.ValidateCharacters(value,
                         allowDots:true,
                         allowHyphens:true,
                         allowUnderscores: false,
                         allowSpaces:false)
                     | Validations<DataStoreName>.ValidateLength(value, 1, 50));
    }
}
