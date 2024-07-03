using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public class GeneName : EryphName<GeneName>
{
    public GeneName(string value) : base(value)
    {
        ValidOrThrow(Validations<GeneName>.ValidateCharacters(
                         value,
                         allowDots: true,
                         allowHyphens: true,
                         allowUnderscores: false,
                         allowSpaces: false)
                     | Validations<GeneName>.ValidateLength(value, 1, 20));
    }
}
