using System;
using System.Collections.Generic;
using System.Text;
using Dbosoft.Functional.Validations;
using Eryph.ConfigModel.Catlets;
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
        | ValidateProperty(toValidate, c => c.Type,
            s => ValidateWhenAllowed(s, toValidate, ValidateFodderType, "fodder type"), path)
        | ValidateProperty(toValidate, c => c.Content,
            s => ValidateWhenAllowed(s, toValidate, Success<Error, string>, "content"), path)
        | ValidateProperty(toValidate, c => c.FileName,
            s => ValidateWhenAllowed(s, toValidate, s2 => ValidateFileName(s2, "file name"), "file name"), path)
        | ValidateProperty(toValidate, c => c.Secret,
            s => ValidateWhenAllowed(s, toValidate, Success<Error, bool?>, "secret flag"), path);

    private static Validation<Error, string> ValidateWhenAllowed(
        string value,
        FodderConfig config,
        Func<string, Validation<Error, string>> validate,
        string valueName) =>
        ValidateWhenAllowed<string>(notEmpty(value) ? value : null, config, validate, valueName);

    private static Validation<Error, T> ValidateWhenAllowed<T>(
        T value,
        FodderConfig config,
        Func<T, Validation<Error, T>> validate,
        string valueName) =>
        from _ in guardnot(value is not null && config.Remove.GetValueOrDefault(),
                          Error.New($"The {valueName} must not be specified when the fodder is removed."))
                      .ToValidation()
                  | guardnot(value is not null && notEmpty(config.Source),
                          Error.New($"The {valueName} must not be specified when the fodder is a reference."))
                      .ToValidation()
        from __ in validate(value)
        select value;
    
    private static Validation<Error, string> ValidateFodderType(
        string fodderType) =>
        from _ in guard(fodderType
                    is "include-url"
                    or "include-once-url"
                    or "cloud-config-archive"
                    or "upstart-job"
                    or "cloud-config"
                    or "part-handler"
                    or "shellscript"
                    or "cloud-boothook",
                Error.New("The fodder type is not supported."))
            .ToValidation()
        select fodderType;
}
