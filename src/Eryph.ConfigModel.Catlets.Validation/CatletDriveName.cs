using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
    public class CatletDriveName(string value)
        : EryphName<CatletDriveName, IsCatletDriveName>(value);

    public readonly struct IsCatletDriveName : PredWithMessage<string>
    {
        public bool True(string value) =>
            Optional(value)
                .Filter(v => v.Length is >= 1 and <= 50)
                .Filter(v => Regex.IsMatch(v, "^[a-z]+$", RegexOptions.Compiled, TimeSpan.FromSeconds(3)))
                .IsSome;

        public string Message =>
            "The name must be between 1 and 50 lowercase letters";
    }
}
