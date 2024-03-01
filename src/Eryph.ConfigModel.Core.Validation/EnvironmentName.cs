using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.ClassInstances.Pred;
using LanguageExt.Common;

namespace Eryph.ConfigModel
{
    public class EnvironmentName : EryphName<EnvironmentName>
    {
        public EnvironmentName(string value) : base(value)
        {
            _ = ValidOrThrow(
                Validations<EnvironmentName>.ValidateCharacters(value)
                | Validations<EnvironmentName>.ValidateLength(value, 1, 50));
        }

        public readonly struct Validating : Validating<string>
        {
            public Validation<Error, string> Validate(string value) =>
                Validations<EnvironmentName>.ValidateCharacters(value)
                | Validations<EnvironmentName>.ValidateLength(value, 1, 50);
        }
    }
}
