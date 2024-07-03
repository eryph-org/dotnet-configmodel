using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public class EryphNetworkName : EryphName<EryphNetworkName>
{
    public EryphNetworkName(string value) : base(value)
    {
        ValidOrThrow(Validations<EryphNetworkName>.ValidateCharacters(
                         value,
                         allowDots: false,
                         allowHyphens: true,
                         allowUnderscores: false,
                         allowSpaces: false)
                     | Validations<EryphNetworkName>.ValidateLength(value, 1, 50));
    }
}