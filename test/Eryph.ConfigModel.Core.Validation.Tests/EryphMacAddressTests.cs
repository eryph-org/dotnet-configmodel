namespace Eryph.ConfigModel.Core.Validation.Tests;

public class EryphMacAddressTests
{
    [Theory]
    [InlineData("1a2b3c4d5e6f")]
    [InlineData("1A2B3C4D5E6F")]
    [InlineData("1a:2b:3c:4d:5e:6f")]
    [InlineData("1A:2B:3C:4D:5E:6F")]
    [InlineData("1a-2b-3c-4d-5e-6f")]
    [InlineData("1A-2B-3C-4D-5E-6F")]
    public void NewValidation_ValidMacAddress_ReturnsNormalizedMacAddress(
        string macAddress)
    {
        var result = EryphMacAddress.NewValidation(macAddress);

        result.Should().BeSuccess()
            .Which.Value.Should().Be("1a:2b:3c:4d:5e:6f");
    }

    [Theory]
    [InlineData("")]
    [InlineData("1A2B3C4D5E6F7A")]
    [InlineData("1a.2b.3c.4d.5e.6f")]
    [InlineData("invalid mac address")]
    public void NewValidation_InvalidMacAddress_ReturnsFail(string value)
    {
        var result = EryphMacAddress.NewValidation(value);

        result.Should().BeFail();
    }
}
