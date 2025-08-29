namespace Eryph.ConfigModel;

public class EryphSubnetName : EryphName<EryphSubnetName>
{
    public EryphSubnetName(string value) : base(value)
    {
        ValidOrThrow(Validations<EryphSubnetName>.ValidateCharacters(
                         value,
                         allowDots: false,
                         allowHyphens: true,
                         allowUnderscores: false,
                         allowSpaces: false)
                     | Validations<EryphSubnetName>.ValidateLength(value, 1, 50));
    }
}
