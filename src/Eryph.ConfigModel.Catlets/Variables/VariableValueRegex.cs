using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel.Variables;

public static class VariableValueRegex
{
    public static readonly string Boolean = "^(true|false)$";

    public static readonly string Number = @"^-?\d{1,9}(\.\d{0,3})?$";
}
