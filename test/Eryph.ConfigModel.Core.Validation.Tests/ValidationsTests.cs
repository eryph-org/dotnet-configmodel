using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ConfigModel.Core.Validation.Tests;

public class ValidationsTests
{
    [Theory]
    [InlineData("some..test")]
    [InlineData("some--test")]
    [InlineData("some  test")]
    public void ValidateCharacters_ConsecutiveSpecialCharacters_ReturnsFail(string value)
    {
        var result = Validations.ValidateCharacters(value, "value", true, true, true);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The value cannot contain consecutive dots, hyphens or spaces."));
    }

    [Theory]
    [InlineData(".test")]
    [InlineData("-test")]
    [InlineData(" test")]
    public void ValidateCharacters_SpecialCharacterInTheBeginning_ReturnsFail(string value)
    {
        var result = Validations.ValidateCharacters(value, "value", true, true, true);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The value cannot start with a dot, hyphen or space."));
    }

    [Theory]
    [InlineData("test.")]
    [InlineData("test-")]
    [InlineData("test ")]
    public void ValidateCharacters_SpecialCharacterAtTheEnd_ReturnsFail(string value)
    {
        var result = Validations.ValidateCharacters(value, "value", true, true, true);

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The value cannot end with a dot, hyphen or space."));
    }

    [Theory]
    [InlineData(@"Z:\")]
    [InlineData(@"Z:\abc\def")]
    [InlineData(@"Z:\abc\def\")]
    [InlineData(@"\\TESTSRV\abc\def\")]
    public void ValidateWindowsPath_ValidPath_ReturnsSuccess(string path)
    {
        var result = Validations.ValidateWindowsPath(path, "path");

        result.Should().BeSuccess().Which.Should().Be(path);
    }

    [Theory]
    [InlineData(@"abc\def")]
    [InlineData(@"Z:abc\def")]
    public void ValidateWindowsPath_NotRooted_ReturnsFail(string path)
    {
        var result = Validations.ValidateWindowsPath(path, "path");

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The path must be a fully-qualified path."));
    }

    [Theory]
    [InlineData(@"Z:\abc\.\def")]
    [InlineData(@"Z:\abc\..\def")]
    public void ValidateWindowsPath_ContainsRelativeSegments_ReturnsFail(string path)
    {
        var result = Validations.ValidateWindowsPath(path, "path");

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The path must be a path without relative segments."));
    }

    [Fact]
    public void ValidateWindowsPath_ContainsUnixSeparator_ReturnsFail()
    {
        var result = Validations.ValidateWindowsPath(@"Z:\abc/def", "path");

        result.Should().BeFail().Which.Should().SatisfyRespectively(
            error => error.Message.Should()
                .Be("The path must only contain Windows directory separators."));
    }
}
