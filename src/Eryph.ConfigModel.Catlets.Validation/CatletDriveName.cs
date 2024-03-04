using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
    public class CatletDriveName : EryphName<CatletDriveName>
    {
        public CatletDriveName(string value) : base(value)
        {
            ValidOrThrow(Validations<CatletDriveName>.ValidateCharacters(value));
        }
    }
}
