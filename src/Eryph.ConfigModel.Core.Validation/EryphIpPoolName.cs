namespace Eryph.ConfigModel;

public class EryphIpPoolName : EryphName<EryphIpPoolName>
{
    public EryphIpPoolName(string value) : base(value)
    {
        ValidOrThrow(Validations<EryphIpPoolName>.ValidateCharacters(
                         value,
                         allowDots: false,
                         allowHyphens: true,
                         allowUnderscores: false,
                         allowSpaces: false)
                     | Validations<EryphIpPoolName>.ValidateLength(value, 1, 50));
    }
}
