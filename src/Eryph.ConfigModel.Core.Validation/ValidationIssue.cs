using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt.Common;

namespace Eryph.ConfigModel;

public readonly struct ValidationIssue(string member, string message)
{
    public string Member => member;

    public string Message => message;

    public Error ToError() => Error.New(ToString());

    public override string ToString() => $"{member}: {message}";
}
