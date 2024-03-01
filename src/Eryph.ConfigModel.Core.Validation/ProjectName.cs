using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.ClassInstances.Pred;
using LanguageExt.Common;
using LanguageExt.TypeClasses;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
    public class ProjectName(string value)
        : EryphName<ProjectName, ProjectName.Validating>(value)
    {
        public readonly struct Validating : Validating<string>
        {
            public Validation<Error, string> Validate(string value) =>
                Validations<ProjectName>.ValidateCharacters(value)
                | Validations<ProjectName>.ValidateLength(value, 1, 20);
        }
    }
}
