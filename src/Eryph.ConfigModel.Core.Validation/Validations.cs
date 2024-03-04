using LanguageExt.Common;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

#nullable enable

public static class Validations
{
    public static readonly Regex NameRegex = new(@"^[a-z0-9\.\-]*$", RegexOptions.Compiled);

    public static Validation<Error, string> ValidateNotEmpty(string? value, string name) =>
        from nonEmptyValue in Optional(value).Filter(notEmpty).ToValidation(Error.New($"The {name} cannot be empty."))
        select nonEmptyValue;

    public static Validation<Error, string> ValidateLength(string value, string fieldName, int maxLength, int minLength) =>
        from _ in guardnot(value.Length < minLength,
            Error.New($"The {fieldName} is shorter than the minimum length of {minLength} characters."))
            .ToValidation()
        from __ in guardnot(value.Length > maxLength,
            Error.New($"The {fieldName} is longer than the maximum length of {maxLength} characters."))
            .ToValidation()
        select value;

    public static Validation<Error, string> ValidateCharacters(string value, string fieldName) =>
        from _ in guard(value is null || NameRegex.IsMatch(value),
                      Error.New($"The {fieldName} contains invalid characters. Only lower case latin characters, numbers, dots and hyphens are permitted."))
                      .ToValidation()
                  | guardnot(value is not null && (value.Contains("..") || value.Contains("--")),
                          Error.New($"The {fieldName} cannot contain consecutive dots or hyphens."))
                      .ToValidation()
        select value;

    public static Validation<Error, string> ValidatePath(string? value, string fieldName) =>
        from nonEmptyValue in ValidateNotEmpty(value, fieldName)
        from _ in guardnot(Path.GetInvalidPathChars().Intersect(nonEmptyValue).Any(),
                Error.New($"The {fieldName} must be a valid path but contains invalid characters."))
            .ToValidation()
        from __ in guard(Path.GetPathRoot(nonEmptyValue) is not null,
                Error.New($"The {fieldName} must be a fully-qualified path but it is not."))
            .ToValidation()
        from ___ in guardnot(nonEmptyValue.Length > 260,
                Error.New($"The {fieldName} must be a valid path but contains more than 260 characters."))
            .ToValidation()
        select value;
}

public static class Validations<T>
{
    private static readonly Regex UpperCaseRegex = new("[A-Z]", RegexOptions.Compiled);

    internal static string Name => UpperCaseRegex.Replace(typeof(T).Name, match => $" {match.Value.ToLowerInvariant()}").Trim();

    public static Validation<Error, string> ValidateNotEmpty(string? value) =>
        Validations.ValidateNotEmpty(value, Name);

    public static Validation<Error, string> ValidateLength(string value, int minLength, int maxLength) =>
        Validations.ValidateLength(value, Name, maxLength, minLength);

    public static Validation<Error, string> ValidateCharacters(string value) =>
        Validations.ValidateCharacters(value, Name);
}
