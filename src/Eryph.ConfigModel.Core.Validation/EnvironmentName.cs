using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.ClassInstances.Pred;
using LanguageExt.Common;

namespace Eryph.ConfigModel;

public class EnvironmentName : EryphName<EnvironmentName>
{
    public EnvironmentName(string value) : base(value)
    {
        ValidOrThrow(Validations<EnvironmentName>.ValidateCharacters(
                        value,
                        allowDots: true,
                        allowHyphens: true,
                        allowSpaces: false)
                     | Validations<EnvironmentName>.ValidateLength(value, 1, 50));
    }
}
