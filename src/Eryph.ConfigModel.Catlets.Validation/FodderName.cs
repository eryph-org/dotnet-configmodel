using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public class FodderName : EryphName<FodderName>
{
    public FodderName(string value) : base(value)
    {
        ValidOrThrow(Validations<FodderName>.ValidateCharacters(
                         value,
                         allowDots: true,
                         allowHyphens: true,
                         allowSpaces: false)
                     | Validations<FodderName>.ValidateLength(value, 1, 50));
    }
}
