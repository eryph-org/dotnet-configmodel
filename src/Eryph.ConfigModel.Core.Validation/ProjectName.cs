using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.ClassInstances.Pred;
using LanguageExt.TypeClasses;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
    public class ProjectName(string value)
        : EryphName<ProjectName, IsEryphProjectName>(value);

    public readonly struct IsEryphProjectName : PredWithMessage<string>
    {
        public bool True(string value) =>
            Optional(value)
                .Filter(v => v.Length is >= 1 and <= 50)
                .Filter(v => !v.StartsWith("p_"))
                .Filter(v => Regex.IsMatch(
                    v,
                    "^[a-z0-9]+(-[a-z0-9]+)*$",
                    RegexOptions.Compiled,
                    TimeSpan.FromSeconds(3)))
                .IsSome;

        public string Message =>
            "The name must be between 1 and 50 lowercase letters or numbers and cannot start with p_";
    }
}
