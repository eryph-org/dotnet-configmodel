using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

public class VariableName : EryphName<VariableName>
{
    public VariableName(string value) : base(value)
    {
        ValidOrThrow(
            from validName in Validations<VariableName>.ValidateCharacters(
                                  value,
                                  allowDots: false,
                                  allowHyphens: false,
                                  allowSpaces: false)
                              | Validations<VariableName>.ValidateLength(value, 1, 50)
            from _ in guardnot(char.IsDigit(validName[0]),
                    Error.New("The variable name cannot start with a digit."))
                .ToValidation()
            select validName);
    }
}
