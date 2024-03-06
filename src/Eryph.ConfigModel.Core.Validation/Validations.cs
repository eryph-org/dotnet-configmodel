using LanguageExt.Common;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

#nullable enable

public static class Validations
{
    public static Validation<Error, string> ValidateNotEmpty(
        string? value,
        string valueName) =>
        from nonEmptyValue in Optional(value).Filter(notEmpty)
            .ToValidation(Error.New($"The {valueName} cannot be empty."))
        select nonEmptyValue;

    public static Validation<Error, string> ValidateLength(
        string? value,
        string valueName,
        int minLength,
        int maxLength) =>
        from _ in guardnot((value?.Length ?? 0) < minLength,
            Error.New($"The {valueName} is shorter than the minimum length of {minLength} characters."))
            .ToValidation()
        from __ in guardnot((value?.Length ?? 0) > maxLength,
            Error.New($"The {valueName} is longer than the maximum length of {maxLength} characters."))
            .ToValidation()
        select value;

    public static Validation<Error, string> ValidateCharacters(
        string? value,
        string valueName,
        bool allowUpperCase,
        bool allowHyphens,
        bool allowDots,
        bool allowSpaces) =>
        from _ in guard(value.ToSeq().All(c =>
                    c is >= 'a' and <= 'z' or >= '0' and <= '9'
                    || allowUpperCase && c is >= 'A' and <='Z'
                    || allowDots && c == '.'
                    || allowHyphens && c == '-'
                    || allowSpaces && c == ' '),
                Error.New($"The {valueName} contains invalid characters. Only "
                          + JoinItems("and", Seq(
                              Some($"{(allowUpperCase ? "" : "lower case ")}latin characters"),
                              Some("numbers"),
                              Some("dots").Filter(_ => allowDots),
                              Some("hyphens").Filter(_ => allowHyphens),
                              Some("spaces").Filter(_ => allowSpaces)))
                          + " are permitted."))
            .ToValidation()
        from __ in guardnot(value is not null
                            && (value.Contains("..") || value.Contains("--") || value.Contains("  ")),
                Error.New($"The {valueName} cannot contain consecutive "
                          + JoinItems("or", Seq(
                              Some("dots").Filter(_ => allowDots),
                              Some("hyphens").Filter(_ => allowHyphens),
                              Some("spaces").Filter(_ => allowSpaces)))
                          + "."))
            .ToValidation()
        select value;

    private static string JoinItems(
        string lastSeparator,
        Seq<Option<string>> names) =>
        names.Somes().Match(
            Empty: () => "",
            Seq: n => string.Join(
                $" {lastSeparator} ",
                string.Join(", ", n.Take(n.Length - 1)),
                n.Last()));

    public static Validation<Error, string> ValidatePath(
        string? value,
        string fieldName) =>
        from nonEmptyValue in ValidateNotEmpty(value, fieldName)
        from _ in guardnot(Path.GetInvalidPathChars().Intersect(nonEmptyValue).Any(),
                          Error.New($"The {fieldName} must be a valid path but contains invalid characters."))
                      .ToValidation()
                  | guardnot(nonEmptyValue.Length > 260,
                          Error.New($"The {fieldName} must be a valid path but contains more than 260 characters."))
                      .ToValidation()
        from __ in guard(Path.GetPathRoot(nonEmptyValue) is not null,
                Error.New($"The {fieldName} must be a fully-qualified path but it is not."))
            .ToValidation()
        select value;
}

public static class Validations<T>
{
    private static readonly Regex UpperCaseRegex = new("[A-Z]", RegexOptions.Compiled);

    internal static string Name => UpperCaseRegex.Replace(
        typeof(T).Name, match => $" {match.Value.ToLowerInvariant()}").Trim();

    public static Validation<Error, string> ValidateNotEmpty(string? value) =>
        Validations.ValidateNotEmpty(value, Name);

    public static Validation<Error, string> ValidateLength(string value, int minLength, int maxLength) =>
        Validations.ValidateLength(value, Name, minLength, maxLength);

    public static Validation<Error, string> ValidateCharacters(
        string value, bool allowUpperCase, bool allowHyphens, bool allowDots, bool allowSpaces) =>
        Validations.ValidateCharacters(value, Name, allowUpperCase, allowHyphens, allowDots, allowSpaces);
}

#nullable restore
