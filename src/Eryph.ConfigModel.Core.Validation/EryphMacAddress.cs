using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

public class EryphMacAddress : EryphName<EryphMacAddress>
{
    public EryphMacAddress(string value) : base(Normalize(value))
    {
        ValidOrThrow(Validations<EryphMacAddress>.ValidateMacAddress(value));
    }

    private static string Normalize(string value) =>
        Optional(value).Filter(notEmpty)
            .Map(v => v.Replace(":", "").Replace("-", ""))
            .Filter(s => s.Length == 12)
            .Map(s => new string([s[0], s[1], ':', s[2], s[3], ':', s[4], s[5], ':', s[6], s[7], ':', s[8], s[9], ':', s[10], s[11]]))
            .IfNoneUnsafe(value);
}
