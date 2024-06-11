using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel.Variables;

public class VariableBindingConfig
{
    public string? Name { get; set; }

    public string? Value { get; set; }

    public bool? Secret { get; set; }
}
