using System;
using System.Collections.Generic;
using System.Linq;
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
            from _ in guard(
                    value.ToSeq().All(c => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9' or '_'),
                    Error.New("The variable name contains invalid characters. Only latin characters, numbers and underscores are permitted."))
                .ToValidation()
            from __ in guardnot(notEmpty(value) && char.IsDigit(value[0]),
                    Error.New("The variable name cannot start with a digit."))
                .ToValidation()
            select value);
    }
}
