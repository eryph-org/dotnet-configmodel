using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt.ClassInstances.Pred;

namespace Eryph.ConfigModel
{
    public class EnvironmentName(string name)
        : EryphName<EnvironmentName, IsErpyhName>(name);
}
