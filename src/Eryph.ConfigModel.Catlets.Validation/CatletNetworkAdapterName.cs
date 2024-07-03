using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public class CatletNetworkAdapterName : EryphName<CatletNetworkAdapterName>
{
    public CatletNetworkAdapterName(string value) : base(value)
    {
        ValidOrThrow(Validations<CatletNetworkAdapterName>.ValidateCharacters(
                         value,
                         allowDots: false,
                         allowHyphens: true,
                         allowUnderscores: false,
                         allowSpaces: false)
                     // Network adapter names on linux are limited to 15 characters
                     | Validations<CatletNetworkAdapterName>.ValidateLength(value, 2, 15));
    }
}
