using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ConfigModel;

internal static class WindowsPath
{
    // The following code is based on the code in the dotnet runtime
    // source: https://github.com/dotnet/runtime/blob/53af66a48215f0b7833dce499343eccae1dc9dff/src/libraries/System.Private.CoreLib/src/System/IO/Path.Windows.cs
    // Licensed under the MIT License by the .NET Foundation.

    public static char[] GetInvalidFileNameChars() =>
    [
        '\"', '<', '>', '|', '\0',
        (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
        (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
        (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
        (char)31, ':', '*', '?', '\\', '/'
    ];

    public static char[] GetInvalidPathChars() =>
    [
        '|', '\0',
        (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
        (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
        (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
        (char)31,
        // Block some additional characters which have special meaning (e.g. wild cards)
        // and should not be used in standard paths.
        '\"', '<', '>', '*', '?'
    ];
}
