using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public class StorageIdentifier : EryphName<StorageIdentifier>
{
    public StorageIdentifier(string value) : base(value)
    {
        ValidOrThrow(Validations<StorageIdentifier>.ValidateCharacters(
                        value,
                        allowDots: true,
                        allowHyphens: true,
                        allowUnderscores: false,
                        allowSpaces: false)
                     | Validations<StorageIdentifier>.ValidateLength(value, 1, 20));
    }
}
