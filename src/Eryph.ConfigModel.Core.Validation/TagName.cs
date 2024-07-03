using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public class TagName : EryphName<TagName>
{
    public TagName(string value) : base(value)
    {
        ValidOrThrow(Validations<TagName>.ValidateCharacters(
                         value,
                         allowDots: true,
                         allowHyphens: true,
                         allowUnderscores: false,
                         allowSpaces: false)
                     | Validations<TagName>.ValidateLength(value, 3, 20));
    }
}
