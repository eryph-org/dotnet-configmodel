using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;


namespace Eryph.ConfigModel;

public class CatletCapabilityName : EryphName<CatletCapabilityName>
{
    public CatletCapabilityName(string value) : base(value)
    {
        ValidOrThrow(Validations<CatletCapabilityName>.ValidateCharacters(
                         value,
                         allowDots: false,
                         allowHyphens: false,
                         allowUnderscores: true,
                         allowSpaces: false)
                     | Validations<CatletCapabilityName>.ValidateLength(value, 3, 50));
    }
}