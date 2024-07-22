using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public class GeneSetName : EryphName<GeneSetName>
{
    public GeneSetName(string value) : base(value)
    {
        ValidOrThrow(Validations<GeneSetName>.ValidateCharacters(
                         value,
                         allowDots: true,
                         allowHyphens: true,
                         allowUnderscores: false,
                         allowSpaces: false)
                     | Validations<GeneSetName>.ValidateLength(value, 3, 40));
    }
}
