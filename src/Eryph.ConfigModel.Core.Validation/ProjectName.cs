﻿using System;
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
    public class ProjectName : EryphName<ProjectName>
    {
        public ProjectName(string value) : base(value)
        {
            ValidOrThrow(Validations<ProjectName>.ValidateCharacters(value)
                         | Validations<ProjectName>.ValidateLength(value, 1, 20)
                         | Optional(value).Filter(s => !s.StartsWith("p_"))
                             .ToValidation(Error.New("The project name cannot start with 'p_'.")));
        }
    }
}
