using Eryph.ConfigModel.Catlets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel.Variables;

public interface IHasVariableConfig
{
    public VariableConfig[]? Variables { get; set; }
}