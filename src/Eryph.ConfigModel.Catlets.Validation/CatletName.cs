using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel
{
    public class CatletName : EryphName<CatletName>
    {
        public CatletName(string value) : base(value)
        {
            ValidOrThrow(Validations<CatletName>.ValidateCharacters(
                            value,
                            allowDots: true,
                            allowHyphens:true,
                            allowSpaces: false)
                        | Validations<CatletName>.ValidateLength(value, 1, 50));
        }
    }
}
