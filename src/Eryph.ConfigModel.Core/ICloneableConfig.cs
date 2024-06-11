using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public interface ICloneableConfig<out T>
{
    T Clone();
}
