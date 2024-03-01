using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.Common;

namespace Eryph.ConfigModel
{
    public class DataStoreName(string value)
        : EryphName<DataStoreName, DataStoreName.Validating>(value)
    {
        public readonly struct Validating : ConfigModel.Validating<string>
        {
            public Validation<Error, string> Validate(string value) =>
                Validations<DataStoreName>.ValidateCharacters(value)
                | Validations<DataStoreName>.ValidateLength(value, 1, 50);
        }
    }
}
