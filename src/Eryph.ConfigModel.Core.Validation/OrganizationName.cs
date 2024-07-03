using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public class OrganizationName : EryphName<OrganizationName>
{
    public OrganizationName(string value) : base(value)
    {
        ValidOrThrow(Validations<OrganizationName>.ValidateCharacters(
                         value,
                         allowDots: true,
                         allowHyphens: true,
                         allowUnderscores: false,
                         allowSpaces: false)
                     | Validations<OrganizationName>.ValidateLength(value, 3, 40));
    }
}
