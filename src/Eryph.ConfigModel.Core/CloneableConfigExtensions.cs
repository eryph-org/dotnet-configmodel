using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

public static class CloneableConfigExtensions
{
    public static T CloneWith<T>(this T source, Action<T> with) where T : ICloneableConfig<T>
    {
        var result = source.Clone();
        with(result);
        return result;
    }
}