﻿using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

public class ProjectName : EryphName<ProjectName>
{
    public ProjectName(string value) : base(value)
    {
        ValidOrThrow(Validations<ProjectName>.ValidateCharacters(
                         value,
                         allowDots: true,
                         allowHyphens: true,
                         allowUnderscores: false,
                         allowSpaces: false)
                     | Validations<ProjectName>.ValidateLength(value, 1, 20)
                     | Optional(value).Filter(s => !s.StartsWith("p_", StringComparison.OrdinalIgnoreCase))
                         .ToValidation(Error.New("The project name cannot start with 'p_'.")));
    }
}
