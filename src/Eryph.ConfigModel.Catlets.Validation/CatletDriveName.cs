using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
    public class CatletDriveName : EryphName<CatletDriveName>
    {
        public static readonly Regex DriveNameRegex = new(@"^[a-z0-9\-]*$", RegexOptions.Compiled);

        public CatletDriveName(string value) : base(value)
        {
            ValidOrThrow(ValidateCharacters(value)
                         | Validations<CatletDriveName>.ValidateLength(value, 1, 50));
        }

        private static Validation<Error, string> ValidateCharacters(string value) =>
            from _ in guard(value is null || DriveNameRegex.IsMatch(value),
                              Error.New("The catlet drive name contains invalid characters. Only lower case latin characters, numbers and hyphens are permitted."))
                          .ToValidation()
                      | guardnot(value is not null && value.Contains("--"),
                              Error.New($"The catlet drive name cannot contain consecutive dots or hyphens."))
                          .ToValidation()
            select value;
    }
}
