using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using JetBrains.Annotations;
using LanguageExt;

namespace Eryph.ConfigModel.Catlets.Validation.Tests;

public class VariableNameTests
{
    [Theory]
    [InlineData("test")]
    [InlineData("TEST")]
    [InlineData("_test")]
    [InlineData("__test")]
    [InlineData("test0")]
    public void NewValidation_ValidVariableName_ReturnsSuccess(string variableName)
    {
        var result = VariableName.NewValidation(variableName);

        result.Should().BeSuccess()
            .Which.Value.Should().Be(variableName.ToLowerInvariant());
    }

    [Theory]
    [InlineData("te|st")]
    [InlineData("te-st")]
    [InlineData("te.st")]
    [InlineData("te&st")]
    public void NewValidation_VariableNameWithInvalidCharacter_ReturnsFail(string variableName)
    {
        var result = VariableName.NewValidation(variableName);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
        error => error.Message.Should().Be(
            "The variable name contains invalid characters. Only latin characters, numbers and underscores are permitted."));
    }
    [Fact]
    public void NewValidation_VariableNameWithLeadingDigit_ReturnsFail()
    {
        var result = VariableName.NewValidation("0test");

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The variable name cannot start with a digit."));
    }
}
