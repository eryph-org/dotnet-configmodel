﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public class TagName : EryphName<TagName>
{
    public TagName(string value) : base(value)
    {
        _ = ValidOrThrow(
            Validations<TagName>.ValidateCharacters(
                value,
                allowUpperCase: false,
                allowDots: true,
                allowHyphens: true,
                allowSpaces: false)
            | Validations<TagName>.ValidateLength(value, 3, 20));
    }
}
