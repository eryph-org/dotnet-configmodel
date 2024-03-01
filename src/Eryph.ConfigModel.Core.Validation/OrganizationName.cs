using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel
{
    public class OrganizationName : EryphName<OrganizationName>
    {
        public OrganizationName(string value) : base(value)
        {
            _ = ValidOrThrow(
                Validations<OrganizationName>.ValidateCharacters(value)
                | Validations<OrganizationName>.ValidateLength(value, 3, 40));
        }
    }
}
