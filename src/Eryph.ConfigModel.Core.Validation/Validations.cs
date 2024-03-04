using LanguageExt.Common;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
#nullable enable
    public static class Validations
    {
        public static readonly Regex NameRegex = new(@"^[a-z0-9\.\-]*$", RegexOptions.Compiled);
        public static readonly Regex HashRegex = new("^[a-z0-9]*$", RegexOptions.Compiled);

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
            from _ in guard(NameRegex.IsMatch(value),
                          Error.New($"The {fieldName} contains invalid characters. Only lower case latin characters, numbers, dots and hyphens are permitted"))
                          .ToValidation()
                      | guardnot(value.Contains("..") || value.Contains("--"), Error.New($"The {fieldName} cannot contain consecutive dots or hyphens."))
                          .ToValidation()
                      | guardnot(value.Contains("--"), Error.New($"The {fieldName} cannot contain consecutive hyphens."))
                          .ToValidation()
            select value;
        /*
            NameRegex.IsMatch(value)
                ? Prelude.Success<Error, string>(value)
                : StatusCodeToError(HttpStatusCode.BadRequest,
                    $"{fieldName} does not meet the requirements. Only alphanumeric characters, numbers, dots and non-consecutive hyphens are permitted. Hyphens and dots must not appear at the beginning or end.");


        public static Validation<Error, string> ValidateHash(string value, string fieldName) =>
            HashRegex.IsMatch(value)
                ? Prelude.Success<Error, string>(value)
                : StatusCodeToError(HttpStatusCode.BadRequest,
                    $"{fieldName} does not meet the requirements. Only alphanumeric characters and numbers are permitted.");

        public static Validation<Error, string> ValidateKeyIdString(string value, string fieldName)
        {
            value = value.Replace('-', '+').Replace('_', '/');
            if (value.Length % 4 != 0) value += new string('=', 4 - value.Length % 4);

            return Prelude.Try(() =>
            {
                var guidBytes = Convert.FromBase64String(value);
                return new Guid(guidBytes);
            }).ToEither(_ => Error.New("invalid format"))
            ? Prelude.Success<Error, string>(value)
                : StatusCodeToError(HttpStatusCode.BadRequest,
                $"{fieldName} does not meet the requirements. Value has to be a key id.");
        }
        */
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
}
