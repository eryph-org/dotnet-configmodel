using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using Dbosoft.Functional.Validations;
using Eryph.ConfigModel.Variables;
using JetBrains.Annotations;
using LanguageExt;
using LanguageExt.Common;

using static Dbosoft.Functional.Validations.ComplexValidations;
using static Eryph.ConfigModel.Validations;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

#nullable enable

public static class VariableConfigValidations
{
    private static readonly Regex NumberRegex = new(
        VariableValueRegex.Number,
        RegexOptions.Compiled,
        TimeSpan.FromSeconds(1));

    private static readonly Regex BooleanRegex = new(
        VariableValueRegex.Boolean,
        RegexOptions.Compiled,
        TimeSpan.FromSeconds(1));

    internal static Validation<ValidationIssue, Unit> ValidateVariableConfigs(
        IHasVariableConfig toValidate,
        string path,
        bool allowRemove) =>
        from _  in ValidateList(toValidate, c => c.Variables,
            (v, p) => ValidateVariableConfig(v, p, allowRemove), path)
        from __ in ValidateProperty(toValidate, c => c.Variables,
            variables => ValidateDistinct(variables, v => VariableName.NewValidation(v.Name), "variable name"),
            path)
        select unit;

    private static Validation<ValidationIssue, Unit> ValidateVariableConfig(
        VariableConfig toValidate,
        string path,
        bool allowRemove) =>
        ValidateProperty(toValidate, c => c.Name, ValidateVariableName, path, required: true)
        | ValidateProperty(toValidate, c => c.Remove,
            r => guardnot(r && !allowRemove, Error.New("Removing variables is not supported here."))
                .ToValidation()
                .Map(_ => r),
            path)
        | ValidateProperty(toValidate, c => c.Type,
            (VariableType t) => ValidateNotSpecifiedWhenRemoved(t, toValidate, "variable type"), path)
        | ValidateProperty(toValidate, c => c.Value,
            v => from _ in ValidateNotSpecifiedWhenRemoved(v, toValidate, "value")
                 from __ in ValidateVariableValue(v, toValidate.Type)
                 select v,
            path)
        | ValidateProperty(toValidate, c => c.Secret,
            (bool s) => ValidateNotSpecifiedWhenRemoved(s, toValidate, "secret flag"), path)
        | ValidateProperty(toValidate, c => c.Required,
            (bool r) => ValidateNotSpecifiedWhenRemoved(r, toValidate, "required flag"), path);

    private static Validation<Error, T> ValidateNotSpecifiedWhenRemoved<T>(
        T value,
        VariableConfig config,
        string valueName) =>
        from _ in guardnot(config.Remove.GetValueOrDefault(),
                Error.New($"The {valueName} must not be specified when the variable is removed."))
            .ToValidation()
        select value;

    private static Validation<Error, string> ValidateVariableName(string value) =>
        from validName in VariableName.NewValidation(value)
        from _ in Seq("catletId", "vmId")
            .Map(n => guardnot(
                    validName == VariableName.New(n),
                    Error.New($"The variable '{n}' is an automatically provided system variable and cannot be explicitly defined."))
                .ToValidation())
            .Sequence()
        select value;
    
    public static Validation<Error, string> ValidateVariableValue(
        string value,
        VariableType? variableType) =>
        (variableType ?? VariableType.String) switch
        {
            VariableType.String => value,
            VariableType.Number =>
                from _ in guard(
                        NumberRegex.IsMatch(value),
                        Error.New("The value is not a valid number. The number must be between -999999999.999 and 999999999.999."))
                    .ToValidation()
                select value,
            VariableType.Boolean =>
                from _ in  guard(
                        BooleanRegex.IsMatch(value),
                        Error.New("The value is not a valid boolean. Only 'true' and 'false' are allowed."))
                    .ToValidation()
                select value,
            _ => Error.New($"The variable type {variableType} is not supported"),
        };
}
