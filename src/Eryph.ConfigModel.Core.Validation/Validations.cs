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
    private static readonly Regex DriveRootRegex = new(@"^[a-zA-Z]:\\", RegexOptions.Compiled);

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
        bool allowHyphens,
        bool allowDots,
        bool allowSpaces) =>
        from _ in guard(value.ToSeq().All(c =>
                    c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9'
                    || allowDots && c == '.'
                    || allowHyphens && c == '-'
                    || allowSpaces && c == ' '),
                Error.New($"The {valueName} contains invalid characters. Only "
                          + JoinItems("and", Seq(
                              Some("latin characters"),
                              Some("numbers"),
                              Some("dots").Filter(_ => allowDots),
                              Some("hyphens").Filter(_ => allowHyphens),
                              Some("spaces").Filter(_ => allowSpaces)))
                          + " are permitted."))
            .ToValidation()
        from __ in guardnot(notEmpty(value) && (value.Contains("..") || value.Contains("--") || value.Contains("  ")),
                           Error.New($"The {valueName} cannot contain consecutive "
                              + JoinItems("or", Seq(
                                  Some("dots").Filter(_ => allowDots),
                                  Some("hyphens").Filter(_ => allowHyphens),
                                  Some("spaces").Filter(_ => allowSpaces)))
                              + "."))
                       .ToValidation()
                   | guardnot(notEmpty(value) && value[0] is '.' or '-' or ' ',
                           Error.New($"The {valueName} cannot start with a "
                              + JoinItems("or", Seq(
                                  Some("dot").Filter(_ => allowDots),
                                  Some("hyphen").Filter(_ => allowHyphens),
                                  Some("space").Filter(_ => allowSpaces)))
                              + "."))
                       .ToValidation()
                   | guardnot(notEmpty(value) && value.Last() is '.' or '-' or ' ',
                           Error.New($"The {valueName} cannot end with a "
                              + JoinItems("or", Seq(
                                  Some("dot").Filter(_ => allowDots),
                                  Some("hyphen").Filter(_ => allowHyphens),
                                  Some("space").Filter(_ => allowSpaces)))
                              + "."))
                       .ToValidation()
        select value;

    public static Validation<Error, string> ValidateWindowsPath(
        string? value,
        string valueName) =>
        // This code in intentionally does not use any System.IO methods
        // as their behavior is OS dependent.
        from nonEmptyValue in ValidateNotEmpty(value, valueName)
        from _ in guardnot(WindowsPath.GetInvalidPathChars().Intersect(nonEmptyValue).Any(),
                          Error.New($"The {valueName} must be a valid Windows path but contains invalid characters."))
                      .ToValidation()
                  | guardnot(nonEmptyValue.Contains('/'),
                          Error.New($"The {valueName} must only contain Windows directory separators."))
                      .ToValidation()
                  | guardnot(nonEmptyValue.Length > 260,
                          Error.New($"The {valueName} must be a valid Windows path but contains more than 260 characters."))
                      .ToValidation()
                  | guard(nonEmptyValue.StartsWith(@"\\") || DriveRootRegex.IsMatch(nonEmptyValue),
                          Error.New($"The {valueName} must be a fully-qualified path."))
                      .ToValidation()
                  | guardnot(nonEmptyValue.Contains(@"\.\") || nonEmptyValue.Contains(@"\..\"),
                          Error.New($"The {valueName} must be a path without relative segments."))
                      .ToValidation()
        select value;

    public static Validation<Error, string> ValidateFileName(
        string? value,
        string valueName) =>
        from nonEmptyValue in ValidateNotEmpty(value, valueName)
        from _ in guardnot(Path.GetInvalidFileNameChars().Intersect(nonEmptyValue).Any(),
                          Error.New($"The {valueName} must be a valid file name but contains invalid characters."))
                      .ToValidation()
                  | guardnot(nonEmptyValue.Length > 255,
                          Error.New($"The {valueName} must be a valid file name but contains more than 255 characters."))
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
        string value, bool allowHyphens, bool allowDots, bool allowSpaces) =>
        Validations.ValidateCharacters(value, Name, allowHyphens, allowDots, allowSpaces);

    public static Validation<Error, string> ValidatePath(string? value) =>
        Validations.ValidateWindowsPath(value, Name);

    public static Validation<Error, string> ValidateFileName(string? value) =>
        Validations.ValidateFileName(value, Name);
}

#nullable restore
