using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.Common;

namespace Eryph.ConfigModel
{
    public class DataStoreName : EryphName<DataStoreName>
    {
        public DataStoreName(string value) : base(value)
        {
            _ = ValidOrThrow(
                Validations<DataStoreName>.ValidateCharacters(value)
                | Validations<DataStoreName>.ValidateLength(value, 1, 50));
        }

        public readonly struct Validating : ConfigModel.Validating<string>
        {
            public Validation<Error, string> Validate(string value) =>
                Validations<DataStoreName>.ValidateCharacters(value)
                | Validations<DataStoreName>.ValidateLength(value, 1, 50);
        }
    }
}
