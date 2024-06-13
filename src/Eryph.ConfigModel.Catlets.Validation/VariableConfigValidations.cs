using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Dbosoft.Functional.Validations;
using Eryph.ConfigModel.Variables;
using JetBrains.Annotations;
using LanguageExt;
using LanguageExt.Common;

using static Dbosoft.Functional.Validations.ComplexValidations;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

public static class VariableConfigValidations
{
    public static Validation<ValidationIssue, Unit> ValidateVariableConfig(
        VariableConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, VariableName.NewValidation, path, required: true)
        | ValidateProperty(toValidate, c => c.Value, v => ValidateVariableValue(v, toValidate.Type), path);

    public static Validation<Error, string> ValidateVariableValue(
        string value,
        VariableType? variableType) =>
        (variableType ?? VariableType.String) switch
        {
            VariableType.String => value,
            VariableType.Number =>
                from _ in guard(
                        Regex.IsMatch(value, @"^-?\d{1,9}(\.\d{0,3})?$", RegexOptions.Compiled, TimeSpan.FromSeconds(1)),
                        Error.New("The value is not a valid number. The number must be between -999999999.999 and 999999999.999."))
                    .ToValidation()
                select value,
            VariableType.Boolean =>
                from _ in  guard(
                        value is "true" or "false",
                        Error.New("The value is not a valid boolean. Only 'true' and 'false' are allowed."))
                    .ToValidation()
                select value,
            _ => Error.New($"The variable type {variableType} is not supported"),
        };
}
