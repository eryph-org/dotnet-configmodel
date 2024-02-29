using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel
{
    public class DataStoreName(string value)
        : EryphName<DataStoreName, IsErpyhName>(value);
}
