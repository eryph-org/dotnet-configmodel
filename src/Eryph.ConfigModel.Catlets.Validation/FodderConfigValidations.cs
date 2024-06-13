using System;
using System.Collections.Generic;
using System.Text;
using Dbosoft.Functional.Validations;
using Eryph.ConfigModel.Catlets;
using JetBrains.Annotations;
using LanguageExt;
using LanguageExt.Common;

using static Dbosoft.Functional.Validations.ComplexValidations;
using static Eryph.ConfigModel.Validations;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

internal static class FodderConfigValidations
{
    public static Validation<ValidationIssue, Unit> ValidateFodderConfig(
        FodderConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, FodderName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Source, GeneIdentifier.NewValidation, path)
        | guard(notEmpty(toValidate.Name) || notEmpty(toValidate.Source),
                new ValidationIssue(path, "The name or source must be specified."))
            .ToValidation()
        | ValidateProperty(toValidate, c => c.Type,
            s => ValidateWhenAllowed(s, toValidate, ValidateFodderType, "fodder type"), path)
        | ValidateProperty(toValidate, c => c.Content,
            s => ValidateWhenAllowed(s, toValidate, Success<Error, string>, "content"), path)
        | ValidateProperty(toValidate, c => c.FileName,
            s => ValidateWhenAllowed(s, toValidate, s2 => ValidateFileName(s2, "file name"), "file name"), path)
        | ValidateProperty(toValidate, c => c.Secret,
            s => ValidateWhenAllowed(s, toValidate, Success<Error, bool?>, "secret flag"), path);

    private static Validation<Error, T> ValidateWhenAllowed<T>(
        T value,
        FodderConfig config,
        Func<T, Validation<Error, T>> validate,
        string valueName) =>
        from valueIsNotEmpty in Success<Error, bool>(value is not null && (value is not string s || notEmpty(s)))
        from _ in guardnot(valueIsNotEmpty && config.Remove.GetValueOrDefault(),
                          Error.New($"The {valueName} must not be specified when the fodder is removed."))
                      .ToValidation()
                  | guardnot(valueIsNotEmpty && notEmpty(config.Source),
                          Error.New($"The {valueName} must not be specified when the fodder is a reference."))
                      .ToValidation()
        from __ in validate(value)
        select value;
    
    private static Validation<Error, string> ValidateFodderType(
        string fodderType) =>
        from _ in guard(fodderType
                    is "cloud-boothook"
                    or "cloud-config"
                    or "cloud-config-archive"
                    or "include-once-url"
                    or "include-url"
                    or "part-handler"
                    or "shellscript"
                    or "upstart-job",
                Error.New("The fodder type is not supported."))
            .ToValidation()
        select fodderType;
}
