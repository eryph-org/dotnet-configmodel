using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
    public class CatletDriveName(string value)
        : EryphName<CatletDriveName, CatletDriveName.Validating>(value)
    {
        public readonly struct Validating : Validating<string>
        {
            public Validation<Error, string> Validate(string value) =>
                Validations<CatletDriveName>.ValidateCharacters(value);
        }
    }
}
