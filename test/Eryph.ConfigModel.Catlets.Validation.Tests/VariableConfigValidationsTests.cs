using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eryph.ConfigModel.Variables;

namespace Eryph.ConfigModel.Catlets.Validation.Tests;

public class VariableConfigValidationsTests
{
    [Theory]
    [InlineData("catletId")]
    [InlineData("vmId")]
    public void ValidateVariableConfig_ReservedVariableName_ReturnsError(string name)
    {
        var config = new VariableConfig()
        {
            Name = name,
        };

        var result = VariableConfigValidations.ValidateVariableConfig(config);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Message.Should().Be($"The variable '{name}' is an automatically provided system variable and cannot be explicitly defined.");
            });
    }

    [Theory]
    [InlineData("true")]
    [InlineData("false")]
    public void ValidateVariableValue_ValidBooleanValue_ReturnsValue(
        string value)
    {
        var result = VariableConfigValidations.ValidateVariableValue(value, VariableType.Boolean);

        result.Should().BeSuccess().Which.Should().Be(value);
    }

    [Theory]
    [InlineData("not a boolean")]
    [InlineData("truefalse")]
    [InlineData("True")]
    [InlineData("False")]
    public void ValidateVariableValue_InvalidBooleanValue_ReturnsError(
        string value)
    {
        var result = VariableConfigValidations.ValidateVariableValue(value, VariableType.Boolean);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Message.Should().Be("The value is not a valid boolean. Only 'true' and 'false' are allowed.");
            });
    }

    [Theory]
    [InlineData("42")]
    [InlineData("-42")]
    [InlineData("4.2")]
    [InlineData("-4.2")]
    public void ValidateVariableValue_ValidNumberValue_ReturnsValue(
        string value)
    {
        var result = VariableConfigValidations.ValidateVariableValue(value, VariableType.Number);

        result.Should().BeSuccess().Which.Should().Be(value);
    }

    [Theory]
    [InlineData("not a number")]
    [InlineData("4.1234")]
    [InlineData("4,2")]
    [InlineData("1000000000000")]
    [InlineData("-1000000000000")]
    public void ValidateVariableValue_InvalidNumberValue_ReturnsError(
        string value)
    {
        var result = VariableConfigValidations.ValidateVariableValue(value, VariableType.Number);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            issue =>
            {
                issue.Message.Should().Be("The value is not a valid number. The number must be between -999999999.999 and 999999999.999.");
            });
    }

    [Theory]
    [InlineData("test value")]
    public void ValidateVariableValue_ValidStringValue_ReturnsValue(
        string value)
    {
        var result = VariableConfigValidations.ValidateVariableValue(value, VariableType.String);

        result.Should().BeSuccess().Which.Should().Be(value);
    }
}
